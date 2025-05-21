import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { TicMatchComponent } from './components/match/tic-match/tic-match.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HomeComponent, TicMatchComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TicTacToe';
}
