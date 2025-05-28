import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';

import { EMPTY, mergeMap, Observable, of } from 'rxjs';

import { StepOneComponent } from './steps/step-one/step-one.component';
import { StepTwoComponent } from './steps/step-two/step-two.component';
import { StepThreeComponent } from './steps/step-three/step-three.component';
import { MatchTypes } from '../../models/match-types.enum';
import { PlayerService } from '../../services/player.service';
import { ICreateTicPlayerCommand, ICreateTicPlayerResponse } from '../../models/player.model';
import { IFormStep, MultiStepFormManager } from './multi-steps-form';
import { MatchService } from '../../services/match.service';
import { IAddTicPlayerToMatchCommand, IAddTicPlayerToMatchResponse, ICreateTicMatchCommand, ICreateTicMatchResponse } from '../../models/match.model';
import { PlayModeTypes } from '../../models/play-mode-types.enum';

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
  public multiStepFormManager: MultiStepFormManager;

  private fb: FormBuilder;
  private playerService: PlayerService;
  private matchService: MatchService;
  private ticPlayerId: string = '';
  private ticMatchId: string = '';
  private router: Router;
  private successRoute: string = 'ticmatch'

  constructor() {
    this.fb = inject(FormBuilder);
    this.playerService = inject(PlayerService);
    this.matchService = inject(MatchService);
    this.router = inject(Router);
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      playMode: [PlayModeTypes.PlayerVsPlayer, Validators.required],
      matchType: [MatchTypes.CREATE],
      matchId: ['']
    });
    this.multiStepFormManager = inject(MultiStepFormManager);
    this.registerFormSteps = this.multiStepFormManager.steps;
    this.currentFormStep = this.multiStepFormManager.currentFormStep;
    this.currentStepNumber = this.multiStepFormManager.currentStepNumber;
  }

  public get fullNameControl(): FormControl {
    return this.form.get('fullName') as FormControl;
  }

  public get nickNameControl(): FormControl {
    return this.form.get('nickname') as FormControl;
  }

  public get playModeControl(): FormControl {
    return this.form.get('playMode') as FormControl;
  }

  public get matchTypeControl(): FormControl {
    return this.form.get('matchType') as FormControl;
  }

  public get matchIdControl(): FormControl {
    return this.form.get('matchId') as FormControl;
  }

  protected onNextStep(): void {
    this.updateSteps();
  }

  protected onPreviousStep(): void {
    this.updateSteps(true);
  }

  protected onCompleteForm(): void {
    this.handleMatch()
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

  private createPlayer(): Observable<ICreateTicPlayerResponse> {
    const createPlayerCommand: ICreateTicPlayerCommand = {
      name: this.fullNameControl.value,
      nickName: this.nickNameControl.value
    }

    return this.playerService.create(createPlayerCommand);
  }

  private createMatch(initialPlayerId: string = ''): Observable<ICreateTicMatchResponse> {
    const createMatchCommand: ICreateTicMatchCommand = {
      playMode: this.playModeControl.value,
      initialPlayerId: initialPlayerId
    }

    return this.matchService.create(createMatchCommand);
  }

  private addPlayer(matchId: string = ''): Observable<IAddTicPlayerToMatchResponse> {
    const addPlayerCommand: IAddTicPlayerToMatchCommand = {
      matchId: matchId ?? this.ticMatchId,
      playerId: this.ticPlayerId
    }

    return this.matchService.addPlayer(addPlayerCommand, this.ticMatchId);
  }

  private handleMatch(): void {
    switch (this.matchTypeControl.value) {
      case MatchTypes.CREATE:
        this.createMatchFlow();
        break;
      case MatchTypes.JOIN:
        this.joinMatchFlow();
        break;
      default:
        break;
    }
  }

  private createMatchFlow(): void {
    this.createPlayer()
      .pipe(
        mergeMap((createTicPlayerResponse: ICreateTicPlayerResponse) => {
          if (!createTicPlayerResponse) {
            return EMPTY;
          }

          this.ticPlayerId = createTicPlayerResponse.id;
          return this.createMatch(this.ticPlayerId)
        }),
        mergeMap((createTicMatchResponse: ICreateTicMatchResponse) => {
          if (!createTicMatchResponse) {
            return EMPTY;
          }

          this.ticMatchId = createTicMatchResponse.matchId;
          return of(true);
        }))
      .subscribe({
        next: (response: any) => {

          console.log('success', response)
          this.router.navigate([this.successRoute])
        },
        error: (error: any) => console.log('error', error)
      });
  }

  private joinMatchFlow(): void {
    this.createPlayer()
      .pipe(
        mergeMap((createTicPlayerResponse: ICreateTicPlayerResponse) => {
          if (!createTicPlayerResponse) {
            return EMPTY;
          }

          this.ticPlayerId = createTicPlayerResponse.id;
          const matchId = this.matchIdControl.value;
          return this.addPlayer(matchId);
        }))
      .subscribe({
        next: (response: any) => {
          debugger;
          console.log('success', response)
          this.router.navigate([this.successRoute])
        },
        error: (error: Error) => console.log('error', error)
      });
  }

  /*
  1 - criação do player;
  2 - escolher o playmode;
  3 - criar a partida;
  4 - adicionar o player na partida;
  (frontend é redirecionado para a tela do tabuleiro).
  5 - Iniciar a partida
  5.a - Se for PlayerVsPlayer => fazer o gerenciamento com o signalr;
    (o segundo player precisa incluir o id da partida na tela e entrar nela);
    A partir deste momento, o tabuleiro será desbloqueado em ambas as telas,
    permitindo que sejam feitas as chamadas de movimento.
  5.b - Se for PlayerVsComputer => a cada movimento do usuário, deve ser gerado 
  um movimento randômico e válido para a partida no backend; Essa informação será
  retornada para a tela (talvez utilizando a técnica de pooling);

  */
}