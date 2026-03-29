import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { NgClass } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, take, Subscription } from 'rxjs';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';
import { TicMatch } from '../shared/models/tic-match.model';
import { IAbandonMatchCommand, IJoinMatchCommand, IMakePlayerMoveCommand, IRematchCommand, ITicMatchStateResponse } from '../shared/services/hub-messages.model';
import { TicMatchState } from '../../home/shared/models/match-state.enum';
import { NotificationService } from '../../../core/notification.service';
import { GameSessionService } from '../../../core/game-session.service';

@Component({
  selector: 'app-tic-match',
  imports: [TicBoardComponent, NgClass],
  standalone: true,
  templateUrl: './tic-match.component.html',
  styleUrl: './tic-match.component.scss'
})
export class TicMatchComponent implements OnInit, OnDestroy {
  protected myPlayerId: string = '';
  protected currentMatchId: string = '';
  protected currentPlayerId: string = '';
  protected currentPlayerSymbol: string = '';
  protected currentMatch: TicMatch | undefined;
  protected showGameOverModal: boolean = false;
  protected showProceedButton: boolean = false;
  protected isComputerThinking: boolean = false;
  private readonly COMPUTER_MOVE_DELAY_MS = 1500;
  protected codeCopied: boolean = false;
  protected winningCells: number[][] | null = null;

  private subscriptions: Subscription[] = [];
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ticMatchHubService = inject(TicMatchHubService);
  private notificationService: NotificationService = inject(NotificationService);
  private gameSessionService: GameSessionService = inject(GameSessionService);

  public get myMatchSymbol(): string {
    if (!this.currentMatch) return '';
    const map: Record<string, string> = {};
    if (this.currentMatch.ticPlayerWithOSymbolId) map[this.currentMatch.ticPlayerWithOSymbolId] = 'O';
    if (this.currentMatch.ticPlayerWithXSymbolId) map[this.currentMatch.ticPlayerWithXSymbolId] = 'X';
    return map[this.myPlayerId] ?? '';
  }

  public get myNickName(): string {
    if (!this.currentMatch) return '';
    if (this.currentMatch.ticPlayerWithXSymbolId === this.myPlayerId) return this.currentMatch.playerXNickName ?? '';
    return this.currentMatch.playerONickName ?? '';
  }

  public get opponentNickName(): string {
    if (!this.currentMatch) return '';
    if (this.currentMatch.ticPlayerWithXSymbolId === this.myPlayerId) return this.currentMatch.playerONickName ?? '';
    return this.currentMatch.playerXNickName ?? '';
  }

  public get isMyTurn(): boolean {
    return this.currentPlayerId === this.myPlayerId;
  }

  public get isVsComputer(): boolean {
    return this.currentMatch?.playMode === 1;
  }

  public get isGameOver(): boolean {
    return this.currentMatch?.isFinished ?? false;
  }

  public get isBoardLocked(): boolean {
    return !this.isMyTurn || this.isGameOver || this.isComputerThinking || this.currentMatch?.state !== TicMatchState.IN_PROGRESS;
  }

  public ngOnInit(): void {
    this.currentMatchId = this.route.snapshot.queryParamMap.get('ticMatchId') ?? '';
    this.myPlayerId = this.route.snapshot.queryParamMap.get('ticPlayerId') ?? '';

    if (!this.currentMatchId || !this.myPlayerId) {
      const session = this.gameSessionService.load();
      if (session) {
        this.currentMatchId = session.matchId;
        this.myPlayerId = session.playerId;
      } else {
        this.notificationService.showError('Nenhuma sessao ativa encontrada.', 'Erro');
        this.router.navigate(['/lobby']);
        return;
      }
    }

    this.ticMatchHubService.clearHandlers();

    this.ticMatchHubService.connectionEstablished
      .pipe(filter(connected => connected), take(1))
      .subscribe(() => {
        this.connectToMatchHub();
        this.registerHubHandlers();
      });
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
    this.ticMatchHubService.clearHandlers();
  }

  public onCellClick(position: { row: number; col: number }): void {
    if (this.isBoardLocked) return;
    const command: IMakePlayerMoveCommand = {
      cellRow: position.row,
      cellCol: position.col,
      matchId: this.currentMatchId,
      playerId: this.myPlayerId
    };
    this.ticMatchHubService.makePlayerMove(command);
  }

  public onAbandonMatch(): void {
    const command: IAbandonMatchCommand = { matchId: this.currentMatchId };
    this.ticMatchHubService.abandonMatch(command);
  }

  public onRematch(): void {
    const command: IRematchCommand = { previousMatchId: this.currentMatchId };
    this.ticMatchHubService.rematch(command);
  }

  public onBackToHome(): void {
    this.gameSessionService.clear();
    this.router.navigate(['/lobby']);
  }

  private connectToMatchHub(): void {
    const command: IJoinMatchCommand = { matchId: this.currentMatchId };
    this.ticMatchHubService.joinMatch(command);
  }

  private registerHubHandlers(): void {
    this.subscriptions.push(
      this.ticMatchHubService.onPlayerJoined().subscribe({
        next: (response: ITicMatchStateResponse) => {
          this.updateMatchState(response);
          if (response.state === TicMatchState.IN_PROGRESS) {
            this.notificationService.showSuccess('Ambos jogadores conectados. Jogo iniciado!', 'Jogo');
          } else {
            this.notificationService.showInfo('Aguardando oponente entrar...', 'Jogo');
          }
        }
      })
    );

    this.subscriptions.push(
      this.ticMatchHubService.onPlayerMadeMove().subscribe({
        next: (response: ITicMatchStateResponse) => {
          if (this.isVsComputer && response.computerMoveRow != null && response.computerMoveCol != null) {
            this.applyMoveWithComputerDelay(response);
          } else {
            this.updateMatchState(response);
            this.handleMoveResult(response);
          }
        }
      })
    );

    this.subscriptions.push(
      this.ticMatchHubService.onMatchAbandoned().subscribe({
        next: (response: ITicMatchStateResponse) => {
          this.updateMatchState(response);
          this.gameSessionService.clear();
          this.showGameOverModal = true;
        }
      })
    );

    this.subscriptions.push(
      this.ticMatchHubService.onMatchRematch().subscribe({
        next: (response: ITicMatchStateResponse) => {
          this.currentMatchId = response.matchId;
          this.gameSessionService.save(this.currentMatchId, this.myPlayerId);
          this.showGameOverModal = false;
          this.showProceedButton = false;
          this.updateMatchState(response);
          this.connectToMatchHub();
          const msg = this.isVsComputer ? 'Nova partida contra o computador!' : 'Nova partida criada! Aguardando jogadores...';
          this.notificationService.showSuccess(msg, 'Revanche');
        }
      })
    );
  }

  public onShowResult(): void {
    this.showGameOverModal = true;
  }

  public onCloseModal(): void {
    this.showGameOverModal = false;
  }

  public isWinningCell(row: number, col: number): boolean {
    if (!this.winningCells) return false;
    return this.winningCells.some(c => c[0] === row && c[1] === col);
  }

  private handleMoveResult(response: ITicMatchStateResponse): void {
    if (response.isFinished) {
      this.gameSessionService.clear();
      if (response.winningCells) {
        this.showProceedButton = true;
      } else {
        this.showGameOverModal = true;
      }
    } else if (this.isMyTurn) {
      this.notificationService.showInfo('Sua vez!', 'Jogo');
    }
  }

  private applyMoveWithComputerDelay(response: ITicMatchStateResponse): void {
    this.isComputerThinking = true;
    const intermediateResponse = { ...response, computerMoveRow: null, computerMoveCol: null };
    const savedBoard = response.board;
    const cpuRow = response.computerMoveRow!;
    const cpuCol = response.computerMoveCol!;

    const boardWithoutCpu = savedBoard.map((row: any[], ri: number) =>
      row.map((cell: any, ci: number) =>
        ri === cpuRow && ci === cpuCol ? { symbol: '', state: 0 } : cell
      )
    );

    this.updateMatchState({ ...response, board: boardWithoutCpu, isFinished: false, winningCells: null });

    setTimeout(() => {
      this.isComputerThinking = false;
      this.updateMatchState(response);
      this.handleMoveResult(response);
    }, this.COMPUTER_MOVE_DELAY_MS);

  }
  public onCopyCode(): void {
    const code = this.currentMatch?.shortCode ?? '';
    navigator.clipboard.writeText(code).then(() => {
      this.codeCopied = true;
      setTimeout(() => this.codeCopied = false, 2000);
    });
  }

  public onShareLink(): void {
    const code = this.currentMatch?.shortCode ?? '';
    const link = window.location.origin + '/join?code=' + code;
    if (navigator.share) {
      navigator.share({ title: 'Jogo da Velha', text: 'Entre na partida! Codigo: ' + code, url: link });
    } else {
      navigator.clipboard.writeText(link).then(() => {
        this.notificationService.showSuccess('Link copiado!', 'Compartilhar');
      });
    }
  }

  private updateMatchState(response: ITicMatchStateResponse): void {
    this.currentMatch = {
      id: response.matchId,
      shortCode: response.shortCode,
      state: response.state,
      board: response.board,
      ticPlayerWithXSymbolId: response.ticPlayerWithXSymbolId,
      ticPlayerWithOSymbolId: response.ticPlayerWithOSymbolId,
      playerXNickName: response.playerXNickName,
      playerONickName: response.playerONickName,
      isFinished: response.isFinished,
      isTie: response.isTie,
      isAbandoned: response.isAbandoned,
      winnerSymbol: response.winnerSymbol,
      winnerPlayerId: response.winnerPlayerId,
      playMode: response.playMode,
      computerDifficulty: response.computerDifficulty
    };
    this.currentPlayerId = response.currentPlayerId;
    this.currentPlayerSymbol = response.currentPlayerSymbol;
    this.winningCells = response.winningCells;
  }
}
