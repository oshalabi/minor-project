import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CowsOverviewComponent } from './cowsOverview.component';

describe('cows-table', () => {
  let component: CowsOverviewComponent;
  let fixture: ComponentFixture<CowsOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CowsOverviewComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CowsOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
