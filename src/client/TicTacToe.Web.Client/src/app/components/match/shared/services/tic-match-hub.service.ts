import { EventEmitter, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';

import { Observable } from 'rxjs';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { IJoinMatchCommand } from './hub-messages.model';

@Injectable({ providedIn: 'root' })
export class TicMatchHubService {
  public connectionEstablished = new EventEmitter<Boolean>();

  private readonly hubUrl = `${environment.apiUrl}Ticmatchhub`;
  private hubConnection?: HubConnection;
  private connectionIsEstablished = false;

  constructor() {
    this.createConnection();
    this.startConnection();
  }

  public joinMatch(joinMatchCommand: IJoinMatchCommand): void {
    this.hubConnection!.invoke('JoinMatchAsync', joinMatchCommand);
  }

  public onPlayerJoined(): Observable<any> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicPlayerJoined', (match) => {
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
  }

  private startConnection(): void {
    this.hubConnection!
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(() => { this.startConnection(); }, 5000);
      });
  }
}
