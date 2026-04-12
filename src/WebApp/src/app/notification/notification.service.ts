import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  showSuccess(message: string = 'Actie succesvol uitgevoerd!'): void {
    Swal.fire({
      toast: true,
      position: 'bottom-end',
      icon: 'success',
      title: message,
      showConfirmButton: false,
      timer: 1000,
    });
  }

  showError(message: string = 'Er is een fout opgetreden.'): void {
    Swal.fire({
      toast: true,
      position: 'bottom-end',
      icon: 'error',
      title: message,
      showConfirmButton: false,
      timer: 1000,
    });
  }
}

