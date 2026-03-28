import { Component, inject, OnInit, OnDestroy } from '@angular/core';
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
  imports: [TicBoardComponent],
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

  public get isGameOver(): boolean {
    return this.currentMatch?.isFinished ?? false;
  }

  public get isBoardLocked(): boolean {
    return !this.isMyTurn || this.isGameOver || this.currentMatch?.state !== TicMatchState.IN_PROGRESS;
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
          this.updateMatchState(response);
          if (response.isFinished) {
            this.gameSessionService.clear();
            this.showGameOverModal = true;
          } else if (this.isMyTurn) {
            this.notificationService.showInfo('Sua vez!', 'Jogo');
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
          this.updateMatchState(response);
          this.connectToMatchHub();
          this.notificationService.showSuccess('Nova partida criada! Aguardando jogadores...', 'Revanche');
        }
      })
    );
  }

  private updateMatchState(response: ITicMatchStateResponse): void {
    this.currentMatch = {
      id: response.matchId,
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
      winnerPlayerId: response.winnerPlayerId
    };
    this.currentPlayerId = response.currentPlayerId;
    this.currentPlayerSymbol = response.currentPlayerSymbol;
  }
}
