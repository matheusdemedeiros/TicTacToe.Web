import { PlayModeTypes } from "./play-mode-types.enum"

export interface ICreateTicMatchCommand {
    playMode: PlayModeTypes,
    initialPlayerId: string
}

export interface ICreateTicMatchResponse {
    matchId: string,
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
}