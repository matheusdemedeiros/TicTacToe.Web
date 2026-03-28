import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';
import { TicMatch } from '../shared/models/tic-match.model';
import { IJoinMatchCommand, IJoinMatchResponse, IMakePlayerMoveCommand, IMakePlayerMoveResponse } from '../shared/services/hub-messages.model';
import { TicMatchState } from '../../home/shared/models/match-state.enum';
import { NotificationService } from '../../../core/notification.service';

@Component({
  selector: 'app-tic-match',
  imports: [TicBoardComponent],
  standalone: true,
  templateUrl: './tic-match.component.html',
  styleUrl: './tic-match.component.scss'
})
export class TicMatchComponent implements OnInit {
  protected mySymbol: string = '';
  protected myPlayerId: string = '';
  protected currentMatchId: string = '';
  protected currentPlayerId: string = '';
  protected currentPlayerSymbol: string = '';

  protected currentMatch: TicMatch | undefined;

  private route = inject(ActivatedRoute);
  private ticMatchHubService = inject(TicMatchHubService);
  private notificationService: NotificationService = inject(NotificationService);

  public get myMatchSymbol(): string {
    if (!this.currentMatch) return '';

    const symbolMap: Record<string, string> = {};
    if (this.currentMatch.ticPlayerWithOSymbolId) {
      symbolMap[this.currentMatch.ticPlayerWithOSymbolId] = 'O';
    }
    if (this.currentMatch.ticPlayerWithXSymbolId) {
      symbolMap[this.currentMatch.ticPlayerWithXSymbolId] = 'X';
    }

    return symbolMap[this.myPlayerId] ?? '';
  }

  public get isMyTurn(): boolean {
    return this.currentPlayerId === this.myPlayerId;
  }

  public ngOnInit(): void {
    this.currentMatchId = this.route.snapshot.queryParamMap.get('ticMatchId')!;
    this.myPlayerId = this.route.snapshot.queryParamMap.get('ticPlayerId')!;

    this.ticMatchHubService.connectionEstablished
      .subscribe({
        next: (response: boolean) => {
          if (response) {
            this.connectToMatchHub();
            this.onPlayerJoined();
            this.onPlayerMadeMove();
          }
        }
      })
  }

  public onCellClick(position: { row: number; col: number }): void {
    if (!this.isMyTurn) {
      this.notificationService.showWarning("It's not your turn.", 'Wait');
      return;
    }

    const command: IMakePlayerMoveCommand = {
      cellRow: position.row,
      cellCol: position.col,
      matchId: this.currentMatchId,
      playerId: this.myPlayerId
    }

    this.ticMatchHubService.makePlayerMove(command);
  }

  private connectToMatchHub(): void {
    const command: IJoinMatchCommand = {
      matchId: this.currentMatchId
    }
    this.ticMatchHubService.joinMatch(command);
  }

  private onPlayerJoined(): void {
    this.ticMatchHubService.onPlayerJoined().subscribe({
      next: (match: IJoinMatchResponse) => {
        this.currentMatch = {
          id: match.matchId,
          state: match.state,
          board: match.board,
          ticPlayerWithXSymbolId: match.ticPlayerWithXSymbolId,
          ticPlayerWithOSymbolId: match.TicPlayerWithOSymbolId
        }
        this.currentPlayerId = match.currentPlayerId;
        this.currentPlayerSymbol = match.currentPlayerSymbol;

        if (match.state === TicMatchState.IN_PROGRESS) {
          this.notificationService.showSuccess('Both players connected. Game started!', 'Game');
        } else {
          this.notificationService.showInfo('Waiting for opponent to join...', 'Game');
        }
      }
    })
  }

  private onPlayerMadeMove(): void {
    this.ticMatchHubService.onPlayerMadeMove().subscribe({
      next: (match: IMakePlayerMoveResponse) => {
        this.currentMatch = {
          id: match.matchId,
          state: match.state,
          board: [...match.board.map(row => [...row])],
        };

        this.currentPlayerId = match.currentPlayerId;
        this.currentPlayerSymbol = match.currentPlayerSymbol;

        if (match.state === TicMatchState.FINISHED) {
          const iWon = match.currentPlayerId !== this.myPlayerId;
          if (iWon) {
            this.notificationService.showSuccess('You won! Congratulations!', 'Game Over');
          } else {
            this.notificationService.showError('You lost. Better luck next time!', 'Game Over');
          }
        } else if (this.isMyTurn) {
          this.notificationService.showInfo('Your turn!', 'Game');
        }
      }
    })
  }
}
