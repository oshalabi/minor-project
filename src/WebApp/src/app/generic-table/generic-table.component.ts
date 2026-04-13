import {
  Component,
  ViewChild,
  AfterViewInit,
  OnInit,
  OnChanges,
  ChangeDetectorRef,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MappedTotals, NutrientType, RemoveFeedTypeModalObject, TableSections } from '../../types';
import {
  MatProgressSpinner,
  MatSpinner,
} from '@angular/material/progress-spinner';
import { MatCard } from '@angular/material/card';
import { RemoveFeedTypeModal } from '../modals/remove-feedtype-modal/remove-feedtype-modal';
import { BasalRationService } from '../basalration/basalration.service';

@Component({
  standalone: true,
  selector: 'app-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatInputModule,
    MatFormFieldModule,
    MatSpinner,
    MatCard,
    MatProgressSpinner,
    RemoveFeedTypeModal,
  ],
})
export class GenericTable implements OnInit, AfterViewInit {
  private _filterValue: string = '';
  @Input() showPaginator: boolean = true; // Standaardwaarde op true

  @Input()
  set filterValue(value: string) {
    this._filterValue = value;
    this.applyFilter(); // Pas de filter direct toe bij een wijziging
  }

  get filterValue(): string {
    return this._filterValue;
  }
  @Input() pageSizeOptions = [5, 10, 15];

  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource();
  @Input() sections: Array<TableSections> = [];
  @Input() pageSize = 5;
  @Input() isLoading: boolean = false;
  @Input() errorMessage: string | null = null;
  @Input() footerCalculationResolver?: (field: string) => number | string;
  @Input() footerLabelResolver?: (field: string) => string | null;

  @Input() tableId: string = '';
  @Input() modalId: string = '';
  @Input() removeFeedTypeModalHeader: string = '';

  selectedRow: RemoveFeedTypeModalObject = { feedTypeId: 0, feedTypeName: '' };

  @Output() cellChange = new EventEmitter<{ row: any; field: string }>();
  @Output() feedtypeDeleted = new EventEmitter<RemoveFeedTypeModalObject>();

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.dataSource.filterPredicate = (data: any, filter: string) =>
      Object.keys(data)
        .reduce(
          (acc, key) => acc + (data[key]?.toString().toLowerCase() || ''),
          ''
        )
        .includes(filter.toLowerCase());
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.cdr.detectChanges();
  }

  applyFilter(): void {
    this.dataSource.filter = this.filterValue.trim().toLowerCase();
  }

  getColumnFields(section: TableSections): string[] {
    return section.columns.map((col) => col.field);
  }

  renderCell(row: any, field: string): string | number {
    if (row[field] !== undefined && row[field] !== null) {
      return row[field];
    }

    if (row.totals) {
      const total = row.totals.find((total: any) => total.field === field);
      if (total) {
        return total.value;
      }
    }

    return '0';
  }
  renderFooterCell(field: string): string | number | undefined {
    const customLabel = this.footerLabelResolver
      ? this.footerLabelResolver(field)
      : null;
    if (customLabel) {
      return customLabel;
    }
    if (this.footerCalculationResolver) {
      return this.footerCalculationResolver(field);
    }
    return undefined;
  }

  shouldRenderFooter(): boolean {
    return !!this.footerCalculationResolver || !!this.footerLabelResolver;
  }
  validateAndSetDefault(row: any, field: string): void {
    const value = row[field];

    if (isNaN(value) || value > 100 || row[field] === undefined) {
      row[field] = 0;
    }
    if (isNaN(value) || value < 0 || row[field] === undefined) {
      row[field] = 0;
    }

    this.onCellChange(row, field);
  }

  clearDefaultValue(row: any, field: string): void {
    if (row[field] === 0) {
      row[field] = '';
    }
  }

  onCellChange(row: any, field: string): void {
    this.cellChange.emit({ row, field });
  }

getCellStyle(row: any, field: string): { [key: string]: string } | null {
  if (row && row.totals && row.totals.length > 0) {
    const cellWarning = row.totals.find((total: any) => total.field === field)?.warning;
    if (cellWarning != null && cellWarning != undefined) {
      switch (cellWarning) {
        case 1:
          return { 'background-color': '#F4B183', color: 'black' };
        case 2:
          return { 'background-color': '#ED7D7D ', color: 'black' };
        default:
          return null; // No style
      }
    }
  }
  return null;
}

  removeFeedType(row: any): void {
    this.feedtypeDeleted.emit(row);
  }
  setModalObject(row: any): void {
    this.selectedRow = {
      feedTypeId: row.feedTypeId,
      feedTypeName: row.feedTypeName,
    };
  }

  handleFeedtypeDeleted(row: RemoveFeedTypeModalObject): void {
    this.feedtypeDeleted.emit(row);
  }
}
