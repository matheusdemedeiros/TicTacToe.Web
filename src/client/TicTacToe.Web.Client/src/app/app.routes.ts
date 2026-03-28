import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LobbyComponent } from './components/lobby/lobby.component';
import { TicMatchComponent } from './components/match/tic-match/tic-match.component';
import { playerSessionGuard } from './core/player-session.guard';
import { gameSessionGuard } from './core/game-session.guard';

export const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    },
    {
        path: 'lobby',
        component: LobbyComponent,
        canActivate: [playerSessionGuard]
    },
    {
        path: 'ticmatch',
        component: TicMatchComponent,
        canActivate: [playerSessionGuard, gameSessionGuard]
    }
];
