import { Component, Input } from '@angular/core';

import { TicBoardCellState } from './models/tic-board-cell-state.enum';

@Component({
  selector: 'app-tic-board-cell',
  imports: [],
  standalone: true,
  templateUrl: './tic-board-cell.component.html',
  styleUrl: './tic-board-cell.component.scss'
})
export class TicBoardCellComponent {
  @Input() public symbol: string = '';
  @Input() public state: TicBoardCellState = TicBoardCellState.BLANK;
}
