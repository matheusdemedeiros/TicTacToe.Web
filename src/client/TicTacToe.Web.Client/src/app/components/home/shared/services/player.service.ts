import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../../environments/environment';
import { Observable } from 'rxjs';

import { ICreateTicPlayerCommand, ICreateTicPlayerResponse } from '../models/player.model';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private http = inject(HttpClient);
  private apiUrl: string = `${environment.apiUrl}api`;

  public create(command: ICreateTicPlayerCommand): Observable<ICreateTicPlayerResponse> {
    return this.http.post<ICreateTicPlayerResponse>(this.apiUrl + '/player', command);
  }
}
