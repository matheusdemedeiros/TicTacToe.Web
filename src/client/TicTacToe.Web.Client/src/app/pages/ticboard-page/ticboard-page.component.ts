import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ticboard-page',
  imports: [],
  standalone: true,
  templateUrl: './ticboard-page.component.html',
  styleUrl: './ticboard-page.component.scss'
})
export class TicboardPageComponent implements OnInit {
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
