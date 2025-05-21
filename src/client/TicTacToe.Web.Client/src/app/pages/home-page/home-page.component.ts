import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatchTypes } from './models/match-types.enum';
import { IRegisterCommand } from './models/register.model';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  public form!: FormGroup;
  public MatchTypes = MatchTypes; // Expor enum para o template

  constructor(private fb: FormBuilder) { }

  public ngOnInit(): void {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      nickname: ['', Validators.required],
      matchType: [MatchTypes.CREATE, Validators.required],
      matchId: ['']
    });

    this.form.get('matchType')?.valueChanges.subscribe(value => {
      const matchIdControl = this.form.get('matchId');
      if (value === MatchTypes.JOIN) {
        matchIdControl?.setValidators([Validators.required]);
      } else {
        matchIdControl?.clearValidators();
      }
      matchIdControl?.updateValueAndValidity();
    });
  }

  public onSubmit(): void {
    const command: IRegisterCommand = {
      fullName: this.form.get('fullName')!.value,
      nickname: this.form.get('nickname')!.value,
      matchType: this.form.get('matchType')!.value,
      matchId: this.form.get('matchId')?.value
    }
  }


  get isJoining() {
    return this.form.get('matchType')?.value === MatchTypes.JOIN;
  }
}
