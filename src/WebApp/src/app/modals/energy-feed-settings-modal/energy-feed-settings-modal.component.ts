import { Component, OnInit } from '@angular/core';
import { EnergyFeedSettingsService } from './energy-feed-settings-modal.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';
import { TopbarService } from '../../topbar/topbar.service';

@Component({
  selector: 'app-energy-feed-settings-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './energy-feed-settings-modal.component.html',
  styleUrls: ['./energy-feed-settings-modal.component.css'],
  providers: [EnergyFeedSettingsService],
})
export class EnergyFeedSettingsModalComponent implements OnInit {
  energyFeeds: { name: string; feedTypeId: number; selectedParity: any; parities: any[] }[] = [];
  isLoading = false;
  errorMessage = '';
  selectedRationId: number | null = null;

  constructor(
    private energyFeedSettingsService: EnergyFeedSettingsService,
    private topbarService: TopbarService
  ) {}

  ngOnInit(): void {
    this.topbarService.getSelectedRation().subscribe((ration) => {
      if (ration) {
        this.selectedRationId = ration.id;
        this.loadEnergyFeeds(this.selectedRationId);
      }
    });
  }

  loadEnergyFeedsOnOpen(rationId: number): void {
    this.loadEnergyFeeds(rationId);
  }

  loadEnergyFeeds(rationId: number): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.energyFeedSettingsService.getEnergyFeedSettings(rationId).subscribe({
      next: (data) => {
        const groupedFeeds = this.groupFeedsByFeedType(data);
        this.energyFeeds = groupedFeeds;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load energy feeds. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  groupFeedsByFeedType(data: any[]) {
    const groupedFeeds: { name: string; feedTypeId: number; selectedParity: any; parities: any[] }[] = [];
  
    data.forEach(feed => {
      const existingFeed = groupedFeeds.find(f => f.feedTypeId === feed.feedTypeId);
  
      if (existingFeed) {
        existingFeed.parities.push({
          parityId: feed.parityId,
          minEnergyFeed: feed.minEnergyFeed,
          maxEnergyFeed: feed.maxEnergyFeed
        });
      } else {
        groupedFeeds.push({
          name: feed.feedTypeName,
          feedTypeId: feed.feedTypeId,
          selectedParity: 4, 
          parities: [{
            parityId: feed.parityId,
            minEnergyFeed: feed.minEnergyFeed,
            maxEnergyFeed: feed.maxEnergyFeed
          }]
        });
      }
    });
  
    groupedFeeds.forEach(feed => {
      feed.parities.sort((a, b) => a.parityId - b.parityId);
    });
  
    return groupedFeeds;
  }
  

  onParityChange(feed: any, parityId: number): void {
    feed.selectedParity = parityId;
  }

  confirm(): void {
    const requests: Observable<any>[] = [];
    this.energyFeeds.forEach(feed => {
      feed.parities.forEach(parity => {
        const payload = {
          rationId: this.selectedRationId,
          feedTypeId: feed.feedTypeId,
          parityId: parity.parityId,
          feedTypeName: feed.name,
          minEnergyFeed: parity.minEnergyFeed,
          maxEnergyFeed: parity.maxEnergyFeed
        };
        const request = this.energyFeedSettingsService.setEnergyFeedSettings(payload);
        requests.push(request);
      });
    });

    forkJoin(requests).subscribe({
      next: (response) => {
        console.log("Settings saved successfully", response);
      },
      error: (err) => {
        console.error("Error saving settings", err);
        this.errorMessage = 'Failed to save energy feeds. Please try again later.';
      }
    });
  }
}
