import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { GameSessionService } from './game-session.service';

export const gameSessionGuard: CanActivateFn = (route) => {
  const gameSession = inject(GameSessionService);
  const router = inject(Router);

  const matchId = route.queryParamMap.get('ticMatchId');
  if (matchId) return true;

  const session = gameSession.load();
  if (session) return true;

  return router.createUrlTree(['/lobby']);
};
