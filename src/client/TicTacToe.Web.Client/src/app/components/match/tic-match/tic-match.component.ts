import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';
import { TicMatch } from '../shared/models/tic-match.model';
import { IJoinMatchCommand, IJoinMatchResponse } from '../shared/services/hub-messages.model';

@Component({
  selector: 'app-tic-match',
  imports: [TicBoardComponent],
  standalone: true,
  templateUrl: './tic-match.component.html',
  styleUrl: './tic-match.component.scss'
})
export class TicMatchComponent implements OnInit {
  protected mySymbol: string = 'O';
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

  public ngOnInit(): void {
    this.currentMatchId = this.route.snapshot.queryParamMap.get('ticMatchId')!;
    this.myPlayerId = this.route.snapshot.queryParamMap.get('ticPlayerId')!;

    this.ticMatchHubService.connectionEstablished
      .subscribe({
        next: (response: boolean) => {
          if (response) {
            this.connectToMatchHub();
            this.onPlayerJoined();
          }
        }
      })
  }

  private connectToMatchHub(): void {
    const command: IJoinMatchCommand = {
      matchId: this.currentMatchId
    }
    this.ticMatchHubService.joinMatch(command);
    console.log('Conectado e joinMatch chamado');
  }

  private onPlayerJoined(): void {
    this.ticMatchHubService.onPlayerJoined().subscribe({
      next: (match: IJoinMatchResponse) => {
        console.log('response do tic event', match)
        debugger;
        this.currentMatch = {
          id: match.matchId,
          state: match.state,
          board: match.board,
        }
        this.currentPlayerId = match.currentPlayerId;
        this.currentPlayerSymbol = match.currentPlayerSymbol;
      }
    })
  }
}
