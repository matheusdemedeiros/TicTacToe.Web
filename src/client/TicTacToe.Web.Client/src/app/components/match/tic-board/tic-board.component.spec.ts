import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicBoardComponent } from './tic-board.component';

describe('TicBoardComponent', () => {
  let component: TicBoardComponent;
  let fixture: ComponentFixture<TicBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicBoardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
