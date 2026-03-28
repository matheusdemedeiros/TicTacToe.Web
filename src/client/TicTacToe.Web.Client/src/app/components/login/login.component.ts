import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { PlayerService } from '../home/shared/services/player.service';
import { PlayerSessionService } from '../../core/player-session.service';
import { NotificationService } from '../../core/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  protected nickName: string = '';

  private router = inject(Router);
  private playerService = inject(PlayerService);
  private playerSession = inject(PlayerSessionService);
  private notificationService = inject(NotificationService);

  public ngOnInit(): void {
    if (this.playerSession.isLoggedIn()) {
      this.router.navigate(['/lobby']);
    }
  }

  public onSubmit(): void {
    if (!this.nickName.trim()) {
      this.notificationService.showWarning('Informe seu nickname.', 'Aviso');
      return;
    }

    this.playerService.create({ name: this.nickName.trim(), nickName: this.nickName.trim() })
      .subscribe({
        next: (response) => {
          this.playerSession.save(response.id, this.nickName.trim());
          this.notificationService.showSuccess('Bem-vindo, ' + this.nickName.trim() + '!', 'Login');
          this.router.navigate(['/lobby']);
        }
      });
  }
}
