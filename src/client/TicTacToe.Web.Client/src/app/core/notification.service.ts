import { inject, Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private readonly toastrService = inject(ToastrService);

  public showSuccess(message: string, title: string = 'Success'): void {
    this.toastrService.success(message, title);
  }

  public showError(message: string, title: string = 'Error'): void {
    this.toastrService.error(message, title);
  }

  public showWarning(message: string, title: string = 'Warning'): void {
    this.toastrService.warning(message, title);
  }

  public showInfo(message: string, title: string = 'Info'): void {
    this.toastrService.info(message, title);
  }
}
