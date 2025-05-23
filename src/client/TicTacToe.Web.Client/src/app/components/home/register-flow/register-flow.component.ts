// Arquivo: register-flow.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StepOneComponent } from './steps/step-one/step-one.component';
import { StepTwoComponent } from './steps/step-two/step-two.component';
import { StepThreeComponent } from './steps/step-three/step-three.component';
import { MatchTypes } from '../models/match-types.enum';

@Component({
  selector: 'app-register-flow',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, StepOneComponent, StepTwoComponent, StepThreeComponent],
  templateUrl: './register-flow.component.html',
  styleUrls: ['./register-flow.component.scss']
})
export class RegisterFlowComponent {
  step = 1;
  form: FormGroup;
  MatchTypes = MatchTypes;
  stepLabels = ['Seus dados', 'Modo de jogo', 'Configuração'];

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      playMode: ['', Validators.required],
      matchType: [''],
      matchId: ['']
    });
  }

  nextStep(): void {
    if (this.step === 1 && this.form.get('fullName')?.valid && this.form.get('nickname')?.valid) {
      this.step++;
    } else if (this.step === 2 && this.form.get('playMode')?.valid) {
      this.step++;
    } else if (this.step === 3 && this.form.get('playMode')?.value === 'friend') {
      const matchTypeValid = this.form.get('matchType')?.valid;
      const matchIdValid = this.form.get('matchType')?.value === MatchTypes.JOIN
        ? this.form.get('matchId')?.valid
        : true;
      if (matchTypeValid && matchIdValid) {
        // Ready to submit or redirect
      }
    }
  }

  previousStep(): void {
    if (this.step > 1) {
      this.step--;
    }
  }

} 