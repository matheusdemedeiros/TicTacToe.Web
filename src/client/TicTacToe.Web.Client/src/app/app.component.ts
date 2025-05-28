import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TicMatchHubService } from './components/match/shared/services/tic-match-hub.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TicTacToe';

  constructor (){
    const service = inject(TicMatchHubService)
    debugger;
    service.connect();
  }
}
