import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnergyFoodComponent } from './energy-food.component';

describe('EnergyFoodComponent', () => {
  let component: EnergyFoodComponent;
  let fixture: ComponentFixture<EnergyFoodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnergyFoodComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(EnergyFoodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
