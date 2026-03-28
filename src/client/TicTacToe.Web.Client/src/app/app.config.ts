import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpErrorInterceptor } from './core/http-error.interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';

import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([httpErrorInterceptor])),
    provideAnimations(),
    provideToastr({ preventDuplicates: true, timeOut: 4000, positionClass: 'toast-top-right', progressBar: true }),
  ]
};
