import { EventEmitter, inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';

import { Observable } from 'rxjs';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { IJoinMatchCommand, IMakePlayerMoveCommand, ITicMatchStateResponse } from './hub-messages.model';
import { NotificationService } from '../../../../core/notification.service';

@Injectable({ providedIn: 'root' })
export class TicMatchHubService {
  public connectionEstablished = new EventEmitter<Boolean>();

  private readonly hubUrl = `${environment.apiUrl}Ticmatchhub`;
  private hubConnection?: HubConnection;
  private connectionIsEstablished = false;
  private notificationService: NotificationService = inject(NotificationService);

  constructor() {
    this.createConnection();
    this.startConnection();
  }

  public joinMatch(joinMatchCommand: IJoinMatchCommand): void {
    this.hubConnection!.invoke('JoinMatchAsync', joinMatchCommand)
      .catch((err: Error) => {
        this.notificationService.showError(err.message || 'Failed to join match.', 'SignalR');
      });
  }

  public makePlayerMove(makePlayerMoveCommand: IMakePlayerMoveCommand): void {
    this.hubConnection!.invoke('MakePlayerMoveAsync', makePlayerMoveCommand)
      .catch((err: Error) => {
        this.notificationService.showError(err.message || 'Failed to make move.', 'SignalR');
      });
  }

  public onPlayerJoined(): Observable<ITicMatchStateResponse> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicPlayerJoined', (match) => {
        observer.next(match);
      });
    });
  }

  public onPlayerMadeMove(): Observable<ITicMatchStateResponse> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicPlayerMadeMove', (match) => {
        observer.next(match);
      });
    });
  }

  public disconnect(): Promise<void> {
    return this.hubConnection?.stop() ?? Promise.resolve();
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .build();

    this.hubConnection.onclose(() => {
      this.connectionIsEstablished = false;
      this.notificationService.showWarning('Connection lost. Attempting to reconnect...', 'Connection');
      setTimeout(() => { this.startConnection(); }, 5000);
    });
  }

  private startConnection(): void {
    this.hubConnection!
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        this.connectionEstablished.emit(true);
      })
      .catch(() => {
        this.notificationService.showError('Unable to connect to game server. Retrying...', 'Connection');
        setTimeout(() => { this.startConnection(); }, 5000);
      });
  }
}
