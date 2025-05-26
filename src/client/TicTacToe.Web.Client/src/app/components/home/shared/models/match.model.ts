import { PlayModeTypes } from "./play-mode-types.enum"

export interface ICreateTicMatchCommand {
    playMode: PlayModeTypes,
    initialPlayerId: string
}

export interface ICreateTicMatchResponse {
    matchId: string,
}
