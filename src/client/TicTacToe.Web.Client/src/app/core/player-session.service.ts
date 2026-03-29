import { Injectable } from '@angular/core';

export interface PlayerSession {
  playerId: string;
  nickName: string;
}

@Injectable({ providedIn: 'root' })
export class PlayerSessionService {
  private readonly storageKey = 'tic-tac-toe-player';

  public save(playerId: string, nickName: string): void {
    localStorage.setItem(this.storageKey, JSON.stringify({ playerId, nickName }));
  }

  public load(): PlayerSession | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as PlayerSession;
    } catch {
      return null;
    }
  }

  public isLoggedIn(): boolean {
    return this.load() !== null;
  }

  public clear(): void {
    localStorage.removeItem(this.storageKey);
  }
}
