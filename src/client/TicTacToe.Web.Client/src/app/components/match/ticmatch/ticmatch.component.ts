import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ticmatch-page',
  imports: [],
  standalone: true,
  templateUrl: './ticmatch.component.html',
  styleUrl: './ticmatch.component.scss'
})
export class TicMatchComponent implements OnInit {
  matchId = 'ABC123';
  currentPlayer = 'O'; // ou 'O'
  public board: string[] = Array(9).fill('X');
  winner: string | null = null;
  public cellIndex: number = 0;

  public ngOnInit(): void { }

  public resetGame(): void {
    this.board = Array(9).fill('');
    this.currentPlayer = 'X';
    this.winner = null;
  }
}
