import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';
import { TicMatchHubService } from '../shared/services/tic-match-hub.service';

@Component({
  selector: 'app-tic-match',
  imports: [TicBoardComponent],
  standalone: true,
  templateUrl: './tic-match.component.html',
  styleUrl: './tic-match.component.scss'
})
export class TicMatchComponent implements OnInit {
  protected currentSymbol: string = 'O';
  protected currentMatchId: string = '';
  protected currentPlayerId: string = '';

  private route: ActivatedRoute;
  private ticMatchHubService: TicMatchHubService;

  constructor() {
    this.route = inject(ActivatedRoute);
    this.ticMatchHubService = inject(TicMatchHubService)
  }

  public ngOnInit(): void {
    this.currentMatchId = this.route.snapshot.queryParamMap.get('ticMatchId')!;
    this.currentPlayerId = this.route.snapshot.queryParamMap.get('ticPlayerId')!;

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
    this.ticMatchHubService.joinMatch(this.currentMatchId);
    console.log('Conectado e joinMatch chamado');
  }

  private onPlayerJoined(): void {
    this.ticMatchHubService.onPlayerJoined().subscribe({
      next: (response: any) => {
        console.log('response do tic event', response)
      }
    })
  }
}
