import { FormGroup } from '@angular/forms';
import { MatchTypes } from '../../models/match-types.enum';
import { Injectable } from '@angular/core';
import { PlayModeTypes } from '../../models/play-mode-types.enum';

export interface IFormStep {
    stepNumber: number;
    stepLabel: string;
    isFirstStep?: boolean;
    isLastStep?: boolean;
    isValid: (form: FormGroup) => boolean;
    onSuccess?: (form: FormGroup) => void;
}
export interface IMultiStepForm {
    steps: IFormStep[],
    isCompleted: boolean,
    isValid: boolean
    currentStepNumber: number,
    currentFormStep: IFormStep,
    nextStep: (form: FormGroup) => void,
    previousStep: () => void,
}

export const registerFormSteps: IFormStep[] = [
    {
        stepNumber: 0,
        isFirstStep: true,
        isLastStep: false,
        stepLabel: 'Seus dados',
        isValid: (form: FormGroup) => {
            return form.get('fullName')!.valid && form.get('nickname')!.valid;
        },
    },
    {
        stepNumber: 1,
        isFirstStep: false,
        isLastStep: false,
        stepLabel: 'Modo de jogo',
        isValid: (form: FormGroup) => {
            return form.get('playMode')!.valid;
        },
    },
    {
        stepNumber: 2,
        isFirstStep: false,
        isLastStep: true,
        stepLabel: 'Configuração',
        isValid: (form: FormGroup) => {
            const playModeValue = form.get('playMode')!.value;

            if (playModeValue !== PlayModeTypes.PlayerVsPlayer) {
                return true;
            }

            const matchTypeValid = form.get('matchType')!.valid;
            const matchIdValid = form.get('matchType')!.value === MatchTypes.JOIN
                ? form.get('matchId')!.valid
                : true;

            return matchTypeValid && matchIdValid;
        },
        onSuccess: (form: FormGroup) => {
            console.log('Formulário pronto para submeter ou redirecionar!');
        }
    },
];

@Injectable({
    providedIn: 'root',
})
export class MultiStepFormManager {
    private _currentStepNumber: number;
    private _steps: IFormStep[];

    constructor() {
        this._steps = registerFormSteps;
        this._currentStepNumber = this._steps[0].stepNumber;
    }

    get steps(): IFormStep[] {
        return this._steps;
    }

    get currentStepNumber(): number {
        return this._currentStepNumber;
    }

    get currentFormStep(): IFormStep {
        return this._steps.find(x => x.stepNumber === this._currentStepNumber) as IFormStep;
    }

    get isCompleted(): boolean {
        return this._currentStepNumber === this._steps[this._steps.length - 1].stepNumber && this.currentFormStep.isValid(new FormGroup({}));
    }

    get isValid(): boolean {
        return this.currentFormStep.isValid(new FormGroup({}));
    }

    public nextStep(form: FormGroup): boolean {
        if (!this.currentFormStep.isValid(form)) {
            return false;
        }

        if (this._currentStepNumber === this._steps[this._steps.length - 1].stepNumber) {
            if (this.currentFormStep.onSuccess) {
                this.currentFormStep.onSuccess(form);
            }
            return true;
        }

        const nextStepIndex = this._steps.findIndex(s => s.stepNumber === this._currentStepNumber) + 1;
        if (nextStepIndex < this._steps.length) {
            this._currentStepNumber = this._steps[nextStepIndex].stepNumber;
            return true;
        }
        return false;
    }

    public previousStep(): boolean {
        if (this._currentStepNumber === this._steps[0].stepNumber) {
            return false;
        }

        const previousStepIndex = this._steps.findIndex(s => s.stepNumber === this._currentStepNumber) - 1;
        if (previousStepIndex >= 0) {
            this._currentStepNumber = this._steps[previousStepIndex].stepNumber;
            return true;
        }
        return false;
    }
}