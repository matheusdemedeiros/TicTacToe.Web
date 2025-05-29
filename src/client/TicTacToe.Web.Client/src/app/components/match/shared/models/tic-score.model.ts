import { TicPlayer } from "../../../home/shared/models/player.model";

export interface TicScore {
    winningSymbol: string,
    winningPlayer?: TicPlayer,
    tie: boolean,
    matchId: string,
}