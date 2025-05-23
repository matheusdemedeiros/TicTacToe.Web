import { Component, input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-step-two',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './step-two.component.html',
  styleUrls: ['./step-two.component.scss']
})
export class StepTwoComponent {
  form = input.required<FormGroup>();

  public fieldInvalid(fieldName: string): boolean {
    const control = this.form().get(fieldName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}
