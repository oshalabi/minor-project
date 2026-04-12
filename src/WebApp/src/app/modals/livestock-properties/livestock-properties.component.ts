import { Component } from '@angular/core';
import { LivestockProperties } from '../../../types';
import { RationService } from '../../ration/ration.service';
import { TopbarService } from '../../topbar/topbar.service';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../notification/notification.service';

@Component({
  selector: 'app-livestock-properties',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './livestock-properties.component.html',
  styleUrl: './livestock-properties.component.css'
})
export class LivestockPropertiesComponent {

  livestockProperties: LivestockProperties = {};
  rationId: number = 0;

  constructor(private rationService: RationService, private topbarService: TopbarService, private notificationService: NotificationService) {}


  ngOnInit(): void {
    this.topbarService.getSelectedRation().subscribe((ration) => {
      if (ration) {
        this.rationId = ration.id;
        this.loadLivestockProperties(ration.id);
      }
    });
  }

  loadLivestockProperties(id: number) {
    this.rationService.GetLivestockProperty(id).subscribe((data) => {
      this.livestockProperties = data;
    });
  }

  confirm() {
    this.rationService.updateLivestockProperties(this.rationId, this.livestockProperties)
      .subscribe({
        next: (response) => {
          this.notificationService.showSuccess("Veestapel kenmerken succesvol gewijzigd")
        },
        error: (error) => {
          this.notificationService.showError(`Error updating livestock properties: ${error}`);
        }
      });
  }
  
}
