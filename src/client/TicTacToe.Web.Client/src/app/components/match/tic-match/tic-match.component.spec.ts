import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicMatchComponent } from './tic-match.component';

describe('TicMatchComponent', () => {
  let component: TicMatchComponent;
  let fixture: ComponentFixture<TicMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicMatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicMatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
