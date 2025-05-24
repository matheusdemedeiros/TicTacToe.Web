// Arquivo: register-flow.component.ts
import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, Form } from '@angular/forms';

import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { StepOneComponent } from './steps/step-one/step-one.component';
import { StepTwoComponent } from './steps/step-two/step-two.component';
import { StepThreeComponent } from './steps/step-three/step-three.component';
import { MatchTypes } from '../../models/match-types.enum';
import { PlayerService } from '../../services/player.service';
import { ICreateTicPlayerCommand, ICreateTicPlayerResponse } from '../../models/player.model';
import { take } from 'rxjs';

@Component({
  selector: 'app-register-flow',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, StepOneComponent, StepTwoComponent, StepThreeComponent],
  templateUrl: './register-flow.component.html',
  styleUrls: ['./register-flow.component.scss']
})
export class RegisterFlowComponent implements OnDestroy {
  step = 1;
  form: FormGroup;
  MatchTypes = MatchTypes;
  stepLabels = ['Seus dados', 'Modo de jogo', 'Configuração'];
  private fb: FormBuilder;
  private playerService: PlayerService;

  constructor() {
    this.fb = inject(FormBuilder);
    this.playerService = inject(PlayerService);

    this.form = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      playMode: ['', Validators.required],
      matchType: [''],
      matchId: ['']
    });
  }

  ngOnDestroy(): void {
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
  public finish(): void {
    this.createPlayer();
  }

  public createPlayer(): void {
    const createPlayerCommand: ICreateTicPlayerCommand = {
      name: this.form.get('fullName')!.value,
      nickName: this.form.get('nickname')!.value
    }

    this.playerService.create(createPlayerCommand)
      .pipe(take(1))
      .subscribe({
        next: (response: ICreateTicPlayerResponse) => {
          console.log('response', response);
        },
        error: (error: any) => {
          console.log('error', error);
        },
      });
    ;
  }
} 