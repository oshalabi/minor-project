import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnergyFeedSettingsModalComponent } from './energy-feed-settings-modal.component';

describe('EnergyFeedSettingsModalComponent', () => {
  let component: EnergyFeedSettingsModalComponent;
  let fixture: ComponentFixture<EnergyFeedSettingsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnergyFeedSettingsModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnergyFeedSettingsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
