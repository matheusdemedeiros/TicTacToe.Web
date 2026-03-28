import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { MatchService } from '../home/shared/services/match.service';
import { PlayerSessionService } from '../../core/player-session.service';
import { GameSessionService } from '../../core/game-session.service';
import { NotificationService } from '../../core/notification.service';
import { PlayModeTypes } from '../home/shared/models/play-mode-types.enum';

@Component({
  selector: 'app-lobby',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.scss'
})
export class LobbyComponent {
  protected playMode: PlayModeTypes = PlayModeTypes.PlayerVsPlayer;
  protected joinMatchId: string = '';
  protected playModeTypes = PlayModeTypes;

  private router = inject(Router);
  private matchService = inject(MatchService);
  private playerSession = inject(PlayerSessionService);
  private gameSession = inject(GameSessionService);
  private notificationService = inject(NotificationService);

  public get nickName(): string {
    return this.playerSession.load()?.nickName ?? '';
  }

  public get playerId(): string {
    return this.playerSession.load()?.playerId ?? '';
  }

  public onCreateMatch(): void {
    this.matchService.create({ playMode: this.playMode, initialPlayerId: this.playerId })
      .subscribe({
        next: (response) => {
          this.gameSession.save(response.matchId, this.playerId);
          this.notificationService.showSuccess('Partida criada!', 'Partida');
          this.router.navigate(['/ticmatch'], { queryParams: { ticMatchId: response.matchId, ticPlayerId: this.playerId } });
        }
      });
  }

  public onJoinMatch(): void {
    if (!this.joinMatchId.trim()) {
      this.notificationService.showWarning('Informe o ID da partida.', 'Aviso');
      return;
    }

    this.matchService.addPlayer({ playerId: this.playerId, matchId: this.joinMatchId.trim() }, this.joinMatchId.trim())
      .subscribe({
        next: (response) => {
          this.gameSession.save(response.matchId, this.playerId);
          this.notificationService.showSuccess('Entrou na partida!', 'Partida');
          this.router.navigate(['/ticmatch'], { queryParams: { ticMatchId: response.matchId, ticPlayerId: this.playerId } });
        }
      });
  }

  public onLogout(): void {
    this.playerSession.clear();
    this.gameSession.clear();
    this.router.navigate(['/']);
  }
}
