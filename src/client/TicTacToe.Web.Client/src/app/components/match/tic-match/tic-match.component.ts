import { Component, OnInit } from '@angular/core';
import { TicBoardComponent } from '../tic-board/tic-board.component';

@Component({
  selector: 'app-tic-match',
  imports: [TicBoardComponent],
  standalone: true,
  templateUrl: './tic-match.component.html',
  styleUrl: './tic-match.component.scss'
})
export class TicMatchComponent implements OnInit {
  matchId = 'ABC123';
  currentPlayer = 'O'; // ou 'O'

  public ngOnInit(): void { }
}
