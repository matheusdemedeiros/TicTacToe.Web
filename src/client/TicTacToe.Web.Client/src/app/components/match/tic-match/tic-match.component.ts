import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TicBoardComponent } from '../tic-board/tic-board.component';

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

  constructor() {
    this.route = inject(ActivatedRoute);
  }

  public ngOnInit(): void {
    this.currentMatchId = this.route.snapshot.queryParamMap.get('ticMatchId')!;
    this.currentPlayerId = this.route.snapshot.queryParamMap.get('ticPlayerId')!;
  }
}
