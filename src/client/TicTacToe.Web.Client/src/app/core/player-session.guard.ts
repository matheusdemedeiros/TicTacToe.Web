import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { PlayerSessionService } from './player-session.service';

export const playerSessionGuard: CanActivateFn = () => {
  const session = inject(PlayerSessionService);
  const router = inject(Router);

  if (session.isLoggedIn()) {
    return true;
  }

  return router.createUrlTree(['/']);
};
