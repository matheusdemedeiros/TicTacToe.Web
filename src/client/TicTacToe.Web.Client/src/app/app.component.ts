import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { TicboardPageComponent } from './pages/ticboard-page/ticboard-page.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HomePageComponent, TicboardPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TicTacToe';
}
