import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

import { NotificationService } from './notification.service';
import { IApiErrorResponse } from './api-error-response.model';

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiError = error.error as IApiErrorResponse;

      if (apiError?.message) {
        notificationService.showError(apiError.message);
      } else if (error.status === 0) {
        notificationService.showError('Unable to connect to the server.', 'Connection Error');
      } else {
        notificationService.showError('An unexpected error occurred.', 'Error');
      }

      return throwError(() => error);
    })
  );
};
