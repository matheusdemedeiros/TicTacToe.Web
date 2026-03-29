import { TicMatchState } from '../../../home/shared/models/match-state.enum'
import { TIicBoardCell } from '../models/tic-board-cell.model'

export interface IJoinMatchCommand {
    matchId: string
}

export interface IMakePlayerMoveCommand {
    matchId: string
    playerId: string,
    cellRow: number,
    cellCol: number,
}

export interface IAbandonMatchCommand {
    matchId: string
}

export interface IRematchCommand {
    previousMatchId: string
}

export interface ITicMatchStateResponse {
    matchId: string,
    shortCode: string,
    board: TIicBoardCell[][],
    state: TicMatchState,
    currentPlayerId: string,
    currentPlayerSymbol: string,
    ticPlayerWithXSymbolId: string,
    ticPlayerWithOSymbolId: string,
    playerXNickName: string | null,
    playerONickName: string | null,
    isFinished: boolean,
    isTie: boolean,
    isAbandoned: boolean,
    winnerSymbol: string | null,
    winnerPlayerId: string,
    playMode: number,
    computerDifficulty: number | null,
    winningCells: number[][] | null,
    computerMoveRow: number | null,
    computerMoveCol: number | null | null
}
