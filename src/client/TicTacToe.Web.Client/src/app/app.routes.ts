import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { TicMatchComponent } from './components/match/tic-match/tic-match.component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    },
    {
        path: 'ticmatch',
        component: TicMatchComponent
    }
];
