import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { ImportService} from './import-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Modal } from 'bootstrap';
import { NotificationService } from '../../notification/notification.service';
import { ImportType } from '../../../types';

@Component({
  selector: 'feedtype-import-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ],
  templateUrl: './import-modal.component.html',
  styleUrl: '../../topbar/topbar.component.css',
})
export class ImportModalComponent {
  @ViewChild('fileModalRef') fileModal!: ElementRef;

  uploadData: Record<ImportType, { file: File | null}> =
    {
      [ImportType.FEEDTYPE]: { file: null },
      [ImportType.NORM]: { file: null },
      [ImportType.COWS]: { file: null },
    };

  importTypes = Object.values(ImportType) as ImportType[]; 
  selectedType: ImportType = ImportType.FEEDTYPE;
  selectedFile: File | null = null; 
  isUploading = false;
  @Input() rationId?: number = 0;

  constructor(private importService: ImportService, private notificationService: NotificationService) {}

  onTypeChange(type: ImportType): void {
    this.selectedType = type; 
    this.selectedFile = null; 
  }
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  uploadFile(fileInput: HTMLInputElement): void {
    console.log(this.selectedFile + this.selectedType)
    if (!this.selectedType || !this.selectedFile) {
      this.notificationService.showError('Selecteer een bestand en een uploadtype!');
      return;
    }

    this.isUploading = true;

    this.importService.uploadFile(this.selectedFile, this.selectedType, this.rationId ?? 0).subscribe({
      next: (response) => {
        console.log(`Bestand succesvol geüpload naar ${this.selectedType}`, response);
        this.notificationService.showSuccess(`Upload succesvol voor ${this.selectedType}!`);
        this.isUploading = false;
        this.selectedFile = null;
        fileInput.value = '';
        this.closeModal();
      },
        error: (error) => {
          console.error(`Upload mislukt voor ${this.selectedType}`, error);
          this.notificationService.showError(`Upload mislukt voor ${this.selectedType}!`);
          this.isUploading = false;
          this.closeModal();
        },
      });
  }

  closeModal(): void {
    const modalElement = this.fileModal.nativeElement;
    const modalInstance =
      Modal.getInstance(modalElement) || new Modal(modalElement);
    modalInstance.hide();


    const backdrops = document.querySelectorAll('.modal-backdrop');
    backdrops.forEach((backdrop) => backdrop.remove());
  }

  openModal(): void {
    this.selectedFile = null; 
    const modalElement = this.fileModal.nativeElement;
    const modalInstance = Modal.getOrCreateInstance(modalElement);
    modalInstance.show();
  }
}
