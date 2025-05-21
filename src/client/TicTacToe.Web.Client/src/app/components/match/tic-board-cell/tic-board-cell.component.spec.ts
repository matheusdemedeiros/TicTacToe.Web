import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicBoardCellComponent } from './tic-board-cell.component';

describe('TicBoardcellComponent', () => {
  let component: TicBoardCellComponent;
  let fixture: ComponentFixture<TicBoardCellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicBoardCellComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(TicBoardCellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
