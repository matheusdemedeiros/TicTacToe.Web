import { Component } from '@angular/core';
import { TicBoardCellState } from './models/tic-board-cell-state.enum';

@Component({
  selector: 'app-tic-board-cell',
  imports: [],
  templateUrl: './tic-board-cell.component.html',
  styleUrl: './tic-board-cell.component.scss'
})
export class TicBoardCellComponent {
  public symbol: string = 'O';
  public state: TicBoardCellState = TicBoardCellState.BLANK;
}
