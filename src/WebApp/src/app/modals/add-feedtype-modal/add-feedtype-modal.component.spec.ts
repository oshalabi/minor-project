import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFeedtypeModalComponent } from './add-feedtype-modal.component';

describe('AddFeedtypeModalComponent', () => {
  let component: AddFeedtypeModalComponent;
  let fixture: ComponentFixture<AddFeedtypeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFeedtypeModalComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AddFeedtypeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
