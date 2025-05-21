import { MatchTypes } from "./match-types.enum";

export interface IRegisterCommand {
    fullName: string,
    nickname: string,
    matchType: MatchTypes,
    matchId?: string
}