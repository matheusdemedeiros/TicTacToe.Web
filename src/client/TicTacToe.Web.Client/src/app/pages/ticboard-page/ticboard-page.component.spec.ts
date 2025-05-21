import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicboardPageComponent } from './ticboard-page.component';

describe('TicboardPageComponent', () => {
  let component: TicboardPageComponent;
  let fixture: ComponentFixture<TicboardPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicboardPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicboardPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
