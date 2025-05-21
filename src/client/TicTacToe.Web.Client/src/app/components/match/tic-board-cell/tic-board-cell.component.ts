import { Component } from '@angular/core';
import { TicBoardCellState } from './models/tic-board-cell-state.enum';

@Component({
  selector: 'app-ticboardcell',
  imports: [],
  templateUrl: './tic-board-cell.component.html',
  styleUrl: './tic-board-cell.component.scss'
})
export class TicBoardCellComponent {
  public symbol: string = '';
  public state: TicBoardCellState = TicBoardCellState.BLANK;
}
