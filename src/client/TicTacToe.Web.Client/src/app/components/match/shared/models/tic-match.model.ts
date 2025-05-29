import { TicMatchState } from "../../../home/shared/models/match-state.enum";
import { PlayModeTypes } from "../../../home/shared/models/play-mode-types.enum";
import { TicPlayer } from "../../../home/shared/models/player.model";
import { TicBoard } from "./tic-board.model";
import { TicScore } from "./tic-score.model";

export interface TicMatch {
    board: TicBoard,
    createdAt: Date,
    id: string,
    playMode: PlayModeTypes,
    players: TicPlayer[]
    state: TicMatchState,
    ticScore: TicScore,
    updatedAd: Date,
}