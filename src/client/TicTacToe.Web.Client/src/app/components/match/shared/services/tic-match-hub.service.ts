import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';

import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class TicMatchHubService {
  private readonly hubConnection: HubConnection;
  private readonly apiUrl: string = environment.apiUrl;
  private readonly hubUrl: string = `${this.apiUrl}Ticmatchhub`;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .build();
  }

  public async connect(): Promise<any> {
    try {
      await this.hubConnection.start();
      console.log('signalr hub conected');
    } catch (error: any){
      
      console.log('signalr throws error', error);
    }
  }
}
