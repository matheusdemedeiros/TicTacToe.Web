import { MatchTypes } from "./match-types.enum";

export interface IRegisterCommand {
    fullName: string,
    nickname: string,
    playMode: string,
    matchType: MatchTypes,
    matchId?: string
}

export interface ICreateMatchCommand {
    fullName: string,
    nickname: string,
    playMode: string,
    matchType: MatchTypes,
    matchId?: string
}