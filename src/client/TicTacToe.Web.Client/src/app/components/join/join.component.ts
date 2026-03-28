import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { mergeMap } from 'rxjs';

import { MatchService } from '../home/shared/services/match.service';
import { PlayerSessionService } from '../../core/player-session.service';
import { GameSessionService } from '../../core/game-session.service';
import { NotificationService } from '../../core/notification.service';

@Component({
  selector: 'app-join',
  standalone: true,
  template: '<div class="bg-black min-h-screen flex items-center justify-center"><p class="text-gray-500">Entrando na partida...</p></div>'
})
export class JoinComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private matchService = inject(MatchService);
  private playerSession = inject(PlayerSessionService);
  private gameSession = inject(GameSessionService);
  private notificationService = inject(NotificationService);

  public ngOnInit(): void {
    const code = this.route.snapshot.queryParamMap.get('code');

    if (!code) {
      this.notificationService.showError('Codigo da partida nao informado.', 'Erro');
      this.router.navigate(['/lobby']);
      return;
    }

    if (!this.playerSession.isLoggedIn()) {
      this.router.navigate(['/'], { queryParams: { returnUrl: '/join?code=' + code } });
      return;
    }

    const playerId = this.playerSession.load()!.playerId;

    this.matchService.resolveByCode(code.trim().toUpperCase())
      .pipe(
        mergeMap((resolved) => this.matchService.addPlayer({ playerId, matchId: resolved.matchId }, resolved.matchId))
      )
      .subscribe({
        next: (response) => {
          this.gameSession.save(response.matchId, playerId);
          this.notificationService.showSuccess('Entrou na partida!', 'Partida');
          this.router.navigate(['/ticmatch'], { queryParams: { ticMatchId: response.matchId, ticPlayerId: playerId } });
        }
      });
  }
}
