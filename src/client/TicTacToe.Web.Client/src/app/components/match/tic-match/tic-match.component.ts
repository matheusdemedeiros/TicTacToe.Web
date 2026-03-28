import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';
import { TicMatch } from '../shared/models/tic-match.model';
import { IJoinMatchCommand, IMakePlayerMoveCommand, ITicMatchStateResponse } from '../shared/services/hub-messages.model';
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
export class TicMatchComponent implements OnInit {
  protected myPlayerId: string = '';
  protected currentMatchId: string = '';
  protected currentPlayerId: string = '';
  protected currentPlayerSymbol: string = '';
  protected currentMatch: TicMatch | undefined;

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

  public get isMyTurn(): boolean {
    return this.currentPlayerId === this.myPlayerId;
  }

  public get isGameOver(): boolean {
    return this.currentMatch?.isFinished ?? false;
  }

  public get isBoardLocked(): boolean {
    return !this.isMyTurn || this.isGameOver || this.currentMatch?.state !== TicMatchState.IN_PROGRESS;
  }

  public get gameResultMessage(): string {
    if (!this.currentMatch?.isFinished) return '';
    if (this.currentMatch.isTie) return 'Draw!';
    if (this.currentMatch.winnerPlayerId === this.myPlayerId) return 'You won!';
    return 'You lost!';
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
        this.notificationService.showError('No active game session found.', 'Error');
        this.router.navigate(['/']);
        return;
      }
    }

    this.ticMatchHubService.connectionEstablished
      .subscribe({
        next: (connected: boolean) => {
          if (connected) {
            this.connectToMatchHub();
            this.onPlayerJoined();
            this.onPlayerMadeMove();
          }
        }
      });
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

  private connectToMatchHub(): void {
    const command: IJoinMatchCommand = { matchId: this.currentMatchId };
    this.ticMatchHubService.joinMatch(command);
  }

  private updateMatchState(response: ITicMatchStateResponse): void {
    this.currentMatch = {
      id: response.matchId,
      state: response.state,
      board: response.board,
      ticPlayerWithXSymbolId: response.ticPlayerWithXSymbolId,
      ticPlayerWithOSymbolId: response.ticPlayerWithOSymbolId,
      isFinished: response.isFinished,
      isTie: response.isTie,
      winnerSymbol: response.winnerSymbol,
      winnerPlayerId: response.winnerPlayerId
    };
    this.currentPlayerId = response.currentPlayerId;
    this.currentPlayerSymbol = response.currentPlayerSymbol;
  }

  private onPlayerJoined(): void {
    this.ticMatchHubService.onPlayerJoined().subscribe({
      next: (response: ITicMatchStateResponse) => {
        this.updateMatchState(response);

        if (response.state === TicMatchState.IN_PROGRESS) {
          this.notificationService.showSuccess('Both players connected. Game started!', 'Game');
        } else {
          this.notificationService.showInfo('Waiting for opponent to join...', 'Game');
        }
      }
    });
  }

  private onPlayerMadeMove(): void {
    this.ticMatchHubService.onPlayerMadeMove().subscribe({
      next: (response: ITicMatchStateResponse) => {
        this.updateMatchState(response);

        if (response.isFinished) {
          this.gameSessionService.clear();
          if (response.isTie) {
            this.notificationService.showWarning('Draw! No winner this time.', 'Game Over');
          } else if (response.winnerPlayerId === this.myPlayerId) {
            this.notificationService.showSuccess('You won! Congratulations!', 'Game Over');
          } else {
            this.notificationService.showError('You lost. Better luck next time!', 'Game Over');
          }
        } else if (this.isMyTurn) {
          this.notificationService.showInfo('Your turn!', 'Game');
        }
      }
    });
  }
}
