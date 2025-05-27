import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../../environments/environment';

import {
  IAddTicPlayerToMatchCommand,
  ICreateTicMatchCommand,
  ICreateTicMatchResponse,
  IAddTicPlayerToMatchResponse as IAddTicPlayerToMatchResponse
} from '../models/match.model';

@Injectable({
  providedIn: 'root'
})
export class MatchService {

  private http = inject(HttpClient);
  private apiUrl: string = `${environment.apiUrl}api`;

  public create(command: ICreateTicMatchCommand): Observable<ICreateTicMatchResponse> {
    return this.http.post<ICreateTicMatchResponse>(this.apiUrl + '/match', command);
  }

  public addPlayer(command: IAddTicPlayerToMatchCommand, matchId: string): Observable<IAddTicPlayerToMatchResponse> {
    return this.http.post<IAddTicPlayerToMatchResponse>(this.apiUrl + `/match/{matchId}/add-player`, command);
  }
}
