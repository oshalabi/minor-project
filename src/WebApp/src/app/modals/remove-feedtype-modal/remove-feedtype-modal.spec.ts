import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveFeedTypeModal } from './remove-feedtype-modal';

describe('ConfirmModalComponent', () => {
  let component: RemoveFeedTypeModal;
  let fixture: ComponentFixture<RemoveFeedTypeModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveFeedTypeModal],
    }).compileComponents();

    fixture = TestBed.createComponent(RemoveFeedTypeModal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
