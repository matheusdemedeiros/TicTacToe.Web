import { Component, input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

import { PlayModeTypes } from '../../../../models/play-mode-types.enum';

@Component({
  selector: 'app-step-two',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './step-two.component.html',
  styleUrls: ['./step-two.component.scss']
})
export class StepTwoComponent {
  form = input.required<FormGroup>();
  playModeTypes = PlayModeTypes;

  public fieldInvalid(fieldName: string): boolean {
    const control = this.form().get(fieldName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}
