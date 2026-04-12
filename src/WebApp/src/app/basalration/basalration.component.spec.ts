import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasalrationComponent } from './basalration.component';

describe('BasalrationComponent', () => {
  let component: BasalrationComponent;
  let fixture: ComponentFixture<BasalrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasalrationComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BasalrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
