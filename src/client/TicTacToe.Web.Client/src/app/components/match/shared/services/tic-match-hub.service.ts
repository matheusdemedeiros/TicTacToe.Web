import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TicMatchHubService {
  private hubConnection?: HubConnection;
  private readonly hubUrl = `${environment.apiUrl}Ticmatchhub`;

  public connect(matchId: string): Promise<void> {
    if (this.hubConnection?.state === 'Connected') return Promise.resolve();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}?matchId=${matchId}`)
      .build();

    return this.hubConnection.start();
  }

  public joinMatch(matchId: string): void {
    this.hubConnection!.invoke('JoinMatchAsync', matchId);

  }

  public listenPlayerJoined(): Observable<any> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicPlayerJoined', (match) => {
        observer.next(match);
      });
    });
  }

  public disconnect(): Promise<void> {
    return this.hubConnection?.stop() ?? Promise.resolve();
  }
}
