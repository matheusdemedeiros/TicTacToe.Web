import { TicMatchState } from '../../../home/shared/models/match-state.enum';
import { TIicBoardCell } from './tic-board-cell.model';

export interface TicMatch {
    id: string,
    shortCode: string,
    board: TIicBoardCell[][],
    state: TicMatchState,
    ticPlayerWithXSymbolId: string,
    ticPlayerWithOSymbolId: string,
    playerXNickName: string | null,
    playerONickName: string | null,
    isFinished: boolean,
    isTie: boolean,
    isAbandoned: boolean,
    winnerSymbol: string | null,
    winnerPlayerId: string | null
}
