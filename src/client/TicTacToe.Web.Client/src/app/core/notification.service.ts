import { inject, Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private readonly toastrService;

  constructor(
  ) {
    this.toastrService = inject(ToastrService);
  }

  public showSuccessMessage(message: string, title: string) {
    this.toastrService.success(message, title);
  }
}
