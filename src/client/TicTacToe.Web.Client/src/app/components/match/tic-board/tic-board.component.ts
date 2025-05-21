import { Component } from '@angular/core';
import { TicBoardCellComponent } from '../tic-board-cell/tic-board-cell.component';

@Component({
  selector: 'app-tic-board',
  imports: [TicBoardCellComponent],
  templateUrl: './tic-board.component.html',
  styleUrl: './tic-board.component.scss'
})
export class TicBoardComponent {
  public board: string[] = Array(9).fill('X');
  public cellIndex: number = 0;

}
