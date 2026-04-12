import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopbarService } from '../../topbar/topbar.service';
import { FormsModule } from '@angular/forms';
import { RationService } from '../../ration/ration.service';
import { BasalRationService } from '../../basalration/basalration.service';
import { Category } from '../../../types';

@Component({
  selector: 'app-add-feedtype-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-feedtype-modal.component.html',
  styleUrls: ['./add-feedtype-modal.component.css'],
})
export class AddFeedtypeModalComponent implements OnInit {
  @Output() onRationsAdded = new EventEmitter<number>();
  @Input() categories: Array<Category> = [];
  @Input() isEnergy: boolean = false
  @Input() modalId: string = "";

  availableBasalrations: any[] = [];
  filteredFeedTypes: any[] = [];
  searchTerm: string = '';
  selectedCategoryTypes: number[] = [];
  rationId: number | undefined;
  selectedBasalRations: number[] = [];

  constructor(
    private rationService: RationService,
    private topBarService: TopbarService,
    private basalRationService: BasalRationService
  ) {}

  ngOnInit(): void {
    this.topBarService.getSelectedRation().subscribe((ration) => {
      if (ration) {
        this.rationId = ration.id;
        this.loadBasalFeedTypes();
      }
    });
  }

  clearSelections(): void {
    this.selectedBasalRations = [];
    this.selectedCategoryTypes = [];
    this.filteredFeedTypes = [];
    this.loadBasalFeedTypes();
  }

  onCategoryTypeChange(categoryType: number, event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      if (!this.selectedCategoryTypes.includes(categoryType)) {
        this.selectedCategoryTypes.push(categoryType);
      }
    } else {
      this.selectedCategoryTypes = this.selectedCategoryTypes.filter(
        (id) => id !== categoryType
      );
    }

    this.loadBasalFeedTypes(); // Fetch data whenever feed types change
  }

  loadBasalFeedTypes(): void {
    if (this.rationId != null && this.selectedCategoryTypes.length > 0) {
      this.rationService.getFeedTypeKeys(this.rationId, this.isEnergy).subscribe({
        next: (data) => {         
          this.basalRationService
            .GetAvailableFeedTypes(this.selectedCategoryTypes, data)
            .subscribe({
              next: (data) => {
                this.availableBasalrations = data;
                this.filteredFeedTypes = this.availableBasalrations;
              },
            });
          this.filterFeedTypes();
        },
        error: (err) => {
          console.error('Error fetching basal rations:', err);
        },
      });
    } else {
      this.availableBasalrations = [];
    }
  }

  isSelected(feedType: any): boolean {
    return this.selectedBasalRations.includes(feedType);
  }

  toggleSelection(feedType: any, event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      if (!this.selectedBasalRations.includes(feedType)) {
        this.selectedBasalRations.push(feedType);
      }
    } else {
      this.selectedBasalRations = this.selectedBasalRations.filter(
        (id) => id !== feedType.id
      );
    }
  }

  addRationsToSelected(): void {
    if (this.rationId != null && this.selectedBasalRations.length > 0) {
      this.rationService
        .addFeedTypesToRation(this.rationId, this.selectedBasalRations, this.isEnergy)
        .subscribe({
          next: () => {
            console.log('Basal rations added successfully.');
            this.onRationsAdded.emit(this.rationId); // Emit de data
          },
          error: (err) => {
            console.error('Error adding basal rations:', err);
          },
        });
    } else {
      console.error('No basal rations selected or no ration selected.');
    }
  }

  refreshBasalRations(rationId: number): void {
    this.rationService.getRationById(rationId, false).subscribe({
      next: (updatedRations) => {
        console.log('Updated basal rations after addition:', updatedRations);
        //this.basalRationService.updateBasalRations(updatedRations);
      },
      error: (err) => console.error('Error refreshing basal rations:', err),
    });
  }

  filterFeedTypes() {
    if (this.searchTerm.length == 0)
      this.filteredFeedTypes = this.availableBasalrations;

    this.filteredFeedTypes = this.availableBasalrations.filter((ration) =>
      ration.name.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }
}
