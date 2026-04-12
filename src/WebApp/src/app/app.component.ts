import { Component, ViewChild } from '@angular/core';
import { TopbarComponent } from './topbar/topbar.component';
import { BasalrationComponent } from './basalration/basalration.component';
import { EnergyFoodComponent } from './energy-food/energy-food.component';
import { CowsOverviewComponent } from './cowsOverview/cowsOverview.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TopbarComponent, BasalrationComponent, EnergyFoodComponent, CowsOverviewComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  @ViewChild(BasalrationComponent) basalRationComponent!: BasalrationComponent;
  @ViewChild(EnergyFoodComponent) EnergyFoodComponent!: EnergyFoodComponent;


  id = this.getRationId();

  title = 'WebApp';

  async onRationSelected(ration: { id: number; name: string }): Promise<void> {
    if (this.basalRationComponent) {
      await this.basalRationComponent.updateRationData(ration.id);
      await this.EnergyFoodComponent.updateRationData(ration.id);
    }
  }

  getRationId(): number {
    if (typeof window !== 'undefined') {
      const parts = window.location.pathname.split('/');
      return parseInt(parts[parts.length - 1], 10);
    }
    return 0;
  }
}
