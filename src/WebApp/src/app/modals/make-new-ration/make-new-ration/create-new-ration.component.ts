import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TopbarComponent } from '../../../topbar/topbar.component';
import { RationService } from '../../../ration/ration.service';

@Component({
  selector: 'create-new-ration-modal',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-new-ration.component.html',
  styleUrls: ['./create-new-ration.component.css'],
})
export class CreateNewRationComponent {
  rationName: string = '';
  errorMessage: string = '';

  constructor(
    private rationService: RationService,
    private topbarComponent: TopbarComponent
  ) {}

  confirm() {
    if (!this.rationName.trim()) {
      this.errorMessage = 'Rantsoen naam is verplicht.';
      return;
    }

    this.errorMessage = '';
    this.rationService.createNewRation(this.rationName).subscribe({
      next: (ration: any) => {
        if (ration) {
          this.topbarComponent
            .selectRation(ration)
            .then(() => {
            })
            .catch((error) => {
              console.error('Failed to select ration:', error);
              this.errorMessage = 'Failed to select ration';
            });
        }
      },
      error: (err) => {
        this.errorMessage =
          'Er is een fout opgetreden bij het toevoegen van het rantsoen.';
        console.error(err);
      },
    });
  }
}
