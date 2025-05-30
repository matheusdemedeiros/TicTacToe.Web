import { TicMatchState } from "../../../home/shared/models/match-state.enum"
import { TIicBoardCell } from "../models/tic-board-cell..model"

export interface IJoinMatchCommand {
    matchId: string
}

export interface IJoinMatchResponse {
    matchId: string,
    board: TIicBoardCell[][],
    state: TicMatchState,
    currentPlayerId: string,
    currentPlayerSymbol: string
}

export interface IMakePlayerMoveCommand {
    matchId: string
    playerId: string,
    cellRow: number,
    cellCol: number,
}

export interface IMakePlayerMoveResponse {
    matchId: string,
    board: TIicBoardCell[][],
    state: TicMatchState,
    currentPlayerId: string,
    currentPlayerSymbol: string
}