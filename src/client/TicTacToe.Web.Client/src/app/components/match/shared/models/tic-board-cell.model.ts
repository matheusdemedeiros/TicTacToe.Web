import { TicBoardCellState } from "../../tic-board-cell/models/tic-board-cell-state.enum";

export interface TIicBoardCell {
    symbol: string,
    state: TicBoardCellState
}