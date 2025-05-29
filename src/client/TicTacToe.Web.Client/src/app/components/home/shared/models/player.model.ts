export interface ICreateTicPlayerCommand {
    name: string,
    nickName: string
}

export interface ICreateTicPlayerResponse {
    id: string,
}

export interface TicPlayer {
    name: string,
    nickName: string,
    symbol: string,
}
