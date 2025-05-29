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

  constructor() {
    const service = inject(TicMatchHubService)
    service.connect('E12294C5-AFCE-4C39-A437-4C7031072C64')
  .then(() => {
    if (service['hubConnection']?.state === 'Connected') {
      service.joinMatch('E12294C5-AFCE-4C39-A437-4C7031072C64');
      console.log('Conectado e joinMatch chamado');
    } else {
      console.warn('Conexão não estabelecida');
    }
  })
  .catch(err => console.error('Erro ao conectar ao SignalR', err));

  }
}
