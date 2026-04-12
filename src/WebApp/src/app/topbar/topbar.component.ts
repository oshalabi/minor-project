import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ImportModalComponent } from '../modals/import-modal/import-modal.component';
import { CommonModule, NgFor } from '@angular/common';
import { TopbarService } from './topbar.service';
import { CreateNewRationComponent } from '../modals/make-new-ration/make-new-ration/create-new-ration.component';
import { LivestockPropertiesComponent } from '../modals/livestock-properties/livestock-properties.component';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [ImportModalComponent, CommonModule, NgFor, CreateNewRationComponent, LivestockPropertiesComponent],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss',
})
export class TopbarComponent {
  @Output() rationSelected = new EventEmitter<Ration>();
  private _id!: number;

  @Input()
  set id(value: number) {
    if (value && value !== this._id) {
      this._id = value; // Store the id value
      this.fetchAndSelectRation(value);
    }
  }

  dropdownRations: Ration[] = [];
  selectedRation?: Ration;

  constructor(
    private topbarService: TopbarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getRations();
  }

  selectRation(ration: Ration): Promise<void> {
    return new Promise((resolve, reject) => {
      if (!ration) {
        reject(new Error('Invalid ration input.'));
        return;
      }

      try {
        this.selectedRation = ration; // Update the selected ration
        this.rationSelected.emit(ration); // Emit the selected ration
        this.topbarService.setSelectedRation(ration);

        // Optional: Update the route
        this.router.navigate([`/Ration/BasalRations/${ration.id}`]).then(
          () => resolve(),
          (error) => reject(error) // Handle navigation errors
        );
      } catch (error) {
        reject(error);
      }
    });
  }
  getRations(): void {
    this.topbarService.getRations().subscribe({
      next: (response: Ration[]) => {
        this.dropdownRations = response;
      },
      error: (err) => {
        console.error('Error fetching rations:', err);
      },
    });
  }

  private async fetchAndSelectRation(id: number): Promise<void> {
    try {
      const ration = await this.topbarService.getRation(id).toPromise();
      if (ration) {
        this.selectRation(ration);
      }
    } catch (error) {
      console.error('Error fetching ration by id:', error);
    }
  }

  getSelectedRation(): void {
    this.topbarService.getSelectedRation().subscribe({
      next: (ration) => {
        if (ration) {
          this.selectedRation = ration;
        }
      },
    });
  }
}

export type Ration = {
  id: number;
  name: string;
};
