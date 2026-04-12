import {
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { RemoveFeedTypeModalObject } from '../../../types';

@Component({
  selector: 'remove-feedtype-modal',
  standalone: true,
  templateUrl: './remove-feedtype-modal.html',
  styleUrls: ['./remove-feedtype-modal.css'],
})
export class RemoveFeedTypeModal {
  @Output() onFeedtypeDeleted = new EventEmitter<RemoveFeedTypeModalObject>();

  @Input() modalId!: string;
  @Input() modalObject!: RemoveFeedTypeModalObject;
  @Input() header!: string;

  confirm(): void {
    if ((this.modalObject?.feedTypeId, this.modalObject?.feedTypeName)) {
      this.onFeedtypeDeleted.emit({
        feedTypeName: this.modalObject.feedTypeName,
        feedTypeId: this.modalObject.feedTypeId,
      });
    }
  }
}
