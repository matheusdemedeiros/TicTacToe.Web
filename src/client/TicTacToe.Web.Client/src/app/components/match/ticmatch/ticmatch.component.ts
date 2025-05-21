import { Component, OnInit } from '@angular/core';
import { TicBoardComponent } from '../tic-board/tic-board.component';

@Component({
  selector: 'app-ticmatch-page',
  imports: [TicBoardComponent],
  standalone: true,
  templateUrl: './ticmatch.component.html',
  styleUrl: './ticmatch.component.scss'
})
export class TicMatchComponent implements OnInit {
  matchId = 'ABC123';
  currentPlayer = 'O'; // ou 'O'

  public ngOnInit(): void { }
}
