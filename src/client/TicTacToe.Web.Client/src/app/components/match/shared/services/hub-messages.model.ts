import { TicMatchState } from "../../../home/shared/models/match-state.enum"
import { TIicBoardCell } from "../models/tic-board-cell..model"

export interface IJoinMatchCommand {
    matchId: string
}

export interface IJoinMatchResponse {
    matchId: string,
    board: TIicBoardCell[][],
    state: TicMatchState,
    currentPlaierId: string,
    currentPlayerSymbol: string
}
