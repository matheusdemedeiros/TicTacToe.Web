import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ICreateTicMatchCommand, ICreateTicMatchResponse } from '../models/match.model';

@Injectable({
  providedIn: 'root'
})
export class MatchService {

  private http = inject(HttpClient);
  private apiUrl: string = `${environment.apiUrl}api`;

  public create(command: ICreateTicMatchCommand): Observable<ICreateTicMatchResponse> {
    return this.http.post<ICreateTicMatchResponse>(this.apiUrl + '/match', command);
  }
}
