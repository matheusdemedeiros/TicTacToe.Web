import { Component, EventEmitter, Input, Output } from '@angular/core';

import { TicBoardCellComponent } from '../tic-board-cell/tic-board-cell.component';
import { TicMatch } from '../shared/models/tic-match.model';
import { TIicBoardCell } from '../shared/models/tic-board-cell..model';
import { TicBoardCellState } from '../tic-board-cell/models/tic-board-cell-state.enum';

@Component({
  selector: 'app-tic-board',
  imports: [TicBoardCellComponent],
  standalone: true,
  templateUrl: './tic-board.component.html',
  styleUrl: './tic-board.component.scss'
})
export class TicBoardComponent {
  @Input() currentMatch: TicMatch | undefined;
  @Input() public board: TIicBoardCell[][] | undefined;
  @Output() cellClick = new EventEmitter<{ row: number; col: number }>();

  public handleCellClick(row: number, col: number): void {
    this.cellClick.emit({ row, col });
  }

  public get displayedBoard(): TIicBoardCell[][] {
    return this.currentMatch?.board ?? this.generateBlankBoard();
  }

  private generateBlankBoard(): TIicBoardCell[][] {
    return Array(3).fill(null).map(() =>
      Array(3).fill(null).map(() => ({ symbol: '', state: TicBoardCellState.BLANK }))
    );
  }
}
