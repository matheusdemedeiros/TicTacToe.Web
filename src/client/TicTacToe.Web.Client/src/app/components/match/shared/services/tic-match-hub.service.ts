import { EventEmitter, inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';

import { Observable } from 'rxjs';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { IAbandonMatchCommand, IJoinMatchCommand, IMakePlayerMoveCommand, IRematchCommand, ITicMatchStateResponse } from './hub-messages.model';
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
        this.notificationService.showError(err.message || 'Falha ao entrar na partida.', 'SignalR');
      });
  }

  public abandonMatch(command: IAbandonMatchCommand): void {
    this.hubConnection!.invoke('AbandonMatchAsync', command)
      .catch((err: Error) => {
        this.notificationService.showError(err.message || 'Falha ao abandonar partida.', 'SignalR');
      });
  }

  public rematch(command: IRematchCommand): void {
    this.hubConnection!.invoke('RematchAsync', command)
      .catch((err: Error) => {
        this.notificationService.showError(err.message || 'Falha ao criar revanche.', 'SignalR');
      });
  }

  public onMatchAbandoned(): Observable<ITicMatchStateResponse> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicMatchAbandoned', (match) => {
        observer.next(match);
      });
    });
  }

  public onMatchRematch(): Observable<ITicMatchStateResponse> {
    return new Observable((observer) => {
      this.hubConnection?.on('TicMatchRematch', (match) => {
        observer.next(match);
      });
    });
  }

  public makePlayerMove(makePlayerMoveCommand: IMakePlayerMoveCommand): void {
    this.hubConnection!.invoke('MakePlayerMoveAsync', makePlayerMoveCommand)
      .catch((err: Error) => {
        this.notificationService.showError(err.message || 'Falha ao fazer jogada.', 'SignalR');
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
      this.notificationService.showWarning('Conexao perdida. Reconectando...', 'Conexao');
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
        this.notificationService.showError('Nao foi possivel conectar ao servidor. Tentando novamente...', 'Conexao');
        setTimeout(() => { this.startConnection(); }, 5000);
      });
  }
}
