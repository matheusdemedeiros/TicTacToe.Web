import { Component, Input } from '@angular/core';

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

  public board: TIicBoardCell[] = Array(9).fill({
    symbol: '',
    state: TicBoardCellState.BLANK
  });

  public cellIndex: number = 0;
}
