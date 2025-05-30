import { Component, EventEmitter, Input, Output } from '@angular/core';

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
  @Input() public row: number = 0;
  @Input() public col: number = 0;
  @Output() cellClicked = new EventEmitter<{ row: number; col: number }>();


  public onClick() {
    
    this.cellClicked.emit({ row: this.row, col: this.col })
  }
}
