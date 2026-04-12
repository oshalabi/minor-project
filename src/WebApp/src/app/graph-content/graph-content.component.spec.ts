import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphContentComponent } from './graph-content.component';

describe('GraphContentComponent', () => {
  let component: GraphContentComponent;
  let fixture: ComponentFixture<GraphContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraphContentComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(GraphContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
