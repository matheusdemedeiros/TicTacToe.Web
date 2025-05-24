// Arquivo: register-flow.component.ts
import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { take } from 'rxjs';

import { StepOneComponent } from './steps/step-one/step-one.component';
import { StepTwoComponent } from './steps/step-two/step-two.component';
import { StepThreeComponent } from './steps/step-three/step-three.component';
import { MatchTypes } from '../../models/match-types.enum';
import { PlayerService } from '../../services/player.service';
import { ICreateTicPlayerCommand, ICreateTicPlayerResponse } from '../../models/player.model';
import { IFormStep, MultiStepFormManager } from './multi-steps-form';

@Component({
  selector: 'app-register-flow',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, StepOneComponent, StepTwoComponent, StepThreeComponent],
  templateUrl: './register-flow.component.html',
  styleUrls: ['./register-flow.component.scss']
})
export class RegisterFlowComponent implements OnDestroy {
  public currentStepNumber = 0;
  public currentFormStep: IFormStep;
  public registerFormSteps: IFormStep[];
  public form: FormGroup;
  public MatchTypes = MatchTypes;
  public multiStepFormManager: MultiStepFormManager;

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
    this.multiStepFormManager = inject(MultiStepFormManager);
    this.registerFormSteps = this.multiStepFormManager.steps;
    this.currentFormStep = this.multiStepFormManager.currentFormStep;
    this.currentStepNumber = this.multiStepFormManager.currentStepNumber;
  }

  public onNextStep(): void {
    this.updateSteps();
  }

  public onPreviousStep(): void {
    this.updateSteps(true);
  }

  public onCompleteForm(): void {
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

  public ngOnDestroy(): void {
  }

  private updateSteps(previous: boolean = false): void {
    if (previous) {
      this.multiStepFormManager.previousStep();
    } else {
      this.multiStepFormManager.nextStep(this.form);
    }

    this.currentFormStep = this.multiStepFormManager.currentFormStep;
    this.currentStepNumber = this.multiStepFormManager.currentStepNumber;
  }
} 