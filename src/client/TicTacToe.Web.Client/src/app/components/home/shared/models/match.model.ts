import { PlayModeType } from "./play-mode.enum"

export interface ICreateTicMatchCommand {
    playMode: PlayModeType,
    initialPlayerId: string
}

export interface ICreateTicMatchResponse {
    matchId: string,
}
