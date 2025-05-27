import { Component, input, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PlayModeTypes } from '../../../../models/play-mode-types.enum';

@Component({
  selector: 'app-step-three',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './step-three.component.html',
  styleUrls: ['./step-three.component.scss']
})
export class StepThreeComponent implements OnInit {
  form = input.required<FormGroup>();
  matchTypes = input.required<any>();
  playModeTypes = PlayModeTypes;

  ngOnInit(): void {
    this.form().get('matchType')?.valueChanges.subscribe(value => {
      const matchIdControl = this.form().get('matchId');
      if (value === this.matchTypes().JOIN) {
        matchIdControl?.setValidators([Validators.required]);
      } else {
        matchIdControl?.clearValidators();
      }
      matchIdControl?.updateValueAndValidity();
    });
  }

  fieldInvalid(fieldName: string): boolean {
    const control = this.form().get(fieldName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}
