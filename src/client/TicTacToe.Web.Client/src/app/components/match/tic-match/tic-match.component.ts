import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';
import { TicMatch } from '../shared/models/tic-match.model';
import { IJoinMatchCommand, IJoinMatchResponse, IMakePlayerMoveCommand, IMakePlayerMoveResponse } from '../shared/services/hub-messages.model';

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

  private route: ActivatedRoute;
  private ticMatchHubService: TicMatchHubService;

  constructor() {
    this.route = inject(ActivatedRoute);
    this.ticMatchHubService = inject(TicMatchHubService)
  }

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
      }
    })
  }
}
