import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterFlowComponent } from './register-flow.component';

describe('RegisterFlowComponent', () => {
  let component: RegisterFlowComponent;
  let fixture: ComponentFixture<RegisterFlowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterFlowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterFlowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
