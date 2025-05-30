import { Component, Input } from '@angular/core';
import { TicBoardCellComponent } from '../tic-board-cell/tic-board-cell.component';
import { TicMatch } from '../shared/models/tic-match.model';

@Component({
  selector: 'app-tic-board',
  imports: [TicBoardCellComponent],
  standalone: true,
  templateUrl: './tic-board.component.html',
  styleUrl: './tic-board.component.scss'
})
export class TicBoardComponent {
  @Input() currentMatch: TicMatch | undefined;

  public board: string[] = Array(9).fill('X');
  public cellIndex: number = 0;

}
