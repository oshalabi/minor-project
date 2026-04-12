import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivestockPropertiesComponent } from './livestock-properties.component';

describe('LivestockPropertiesComponent', () => {
  let component: LivestockPropertiesComponent;
  let fixture: ComponentFixture<LivestockPropertiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LivestockPropertiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LivestockPropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
