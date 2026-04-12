import { Component } from '@angular/core';
import { GraphContentComponent } from '../../graph-content/graph-content.component';

@Component({
  selector: 'app-energy-food-modal',
  standalone: true,
  imports: [GraphContentComponent],
  templateUrl: './energy-food-modal.component.html',
  styleUrl: './energy-food-modal.component.css',
})
export class EnergyFoodModalComponent {}
