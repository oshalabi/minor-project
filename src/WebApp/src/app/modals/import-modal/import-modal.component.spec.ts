import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalTestComponent } from './import-modal.component';

describe('ModalTestComponent', () => {
  let component: ModalTestComponent;
  let fixture: ComponentFixture<ModalTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalTestComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ModalTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
