import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnergyFoodModalComponent } from './energy-food-modal.component';

describe('EnergyFoodModalComponent', () => {
  let component: EnergyFoodModalComponent;
  let fixture: ComponentFixture<EnergyFoodModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnergyFoodModalComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(EnergyFoodModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
