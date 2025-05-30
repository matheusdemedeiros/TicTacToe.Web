import { TicMatchState } from "./match-state.enum";
import { PlayModeTypes } from "./play-mode-types.enum"

export interface ICreateTicMatchCommand {
    playMode: PlayModeTypes,
    initialPlayerId: string
}

export interface ICreateTicMatchResponse {
    matchId: string,
    ticPlayerWithXSymbolId: string;
    TicPlayerWithOSymbolId: string;
}

export interface IAddTicPlayerToMatchCommand {
    playerId: string;
    matchId: string;
}

export interface IAddTicPlayerToMatchResponse {
    matchId: string;
    playerId: string;
    playerName: string;
    nickname: string;
    ticPlayerWithXSymbolId: string;
    TicPlayerWithOSymbolId: string;
}

export interface IRetrieveTicMatchByIdResponse {
    found: boolean;
    ticMatchState?: TicMatchState;
    playerNumbers: number;
    matchId: string;
}
