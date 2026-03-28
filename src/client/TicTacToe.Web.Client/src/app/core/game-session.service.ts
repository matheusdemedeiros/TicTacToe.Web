import { Injectable } from '@angular/core';

export interface GameSession {
  matchId: string;
  playerId: string;
}

@Injectable({ providedIn: 'root' })
export class GameSessionService {
  private readonly storageKey = 'tic-tac-toe-session';

  public save(matchId: string, playerId: string): void {
    const session: GameSession = { matchId, playerId };
    localStorage.setItem(this.storageKey, JSON.stringify(session));
  }

  public load(): GameSession | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as GameSession;
    } catch {
      return null;
    }
  }

  public clear(): void {
    localStorage.removeItem(this.storageKey);
  }
}
