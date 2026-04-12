import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateNewRationComponent } from './create-new-ration.component';

describe('MakeNewRationComponent', () => {
  let component: CreateNewRationComponent;
  let fixture: ComponentFixture<CreateNewRationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateNewRationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateNewRationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
