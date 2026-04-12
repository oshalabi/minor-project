import { Component, Input, ViewChild } from '@angular/core';
import { GenericTable } from '../generic-table/generic-table.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BasalRationService } from './basalration.service';
import { AddFeedtypeModalComponent } from '../modals/add-feedtype-modal/add-feedtype-modal.component';
import { MatTableDataSource } from '@angular/material/table';
import {
  FlattenedRationFeedType,
  Ration,
  TableSections,
  NutrientTypeValueEnums,
  Category,
  RemoveFeedTypeModalObject,
  MappedTotals,
} from '../../types';
import { NgClass, NgIf } from '@angular/common';
import { NotificationService } from '../notification/notification.service';

@Component({
  selector: 'app-basalration',
  standalone: true,
  imports: [
    GenericTable,
    MatFormFieldModule,
    AddFeedtypeModalComponent,
    NgClass,
    NgIf,
  ],
  templateUrl: './basalration.component.html',
  styleUrl: './basalration.component.css',
})
export class BasalrationComponent {
  protected readonly GenericTable = GenericTable;
  filterValue: string = '';
  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource();
  nutrients: Array<{ field: string; displayName: string }> = [];
  savedTotal: MappedTotals = { values: [] };
  isCollapsed = false;
  basalrationSections: TableSections[] = [
    {
      header: '',
      stylingClass: 'sticky-container',
      columns: [
        { field: 'feedTypeName', displayName: 'Voersoort' },
        { field: 'kg', displayName: 'KG', editable: true },
        { field: 'kgDs', displayName: 'kg ds', editable: true },
      ],
    },
    {
      header: 'Gemiddelde opname per dier per kg',
      stylingClass: 'sticky-container one',
      columns: [
        {
          field: NutrientTypeValueEnums.Vem.toUpperCase(),
          displayName: NutrientTypeValueEnums.Vem.toUpperCase(),
        },
        {
          field: NutrientTypeValueEnums.Dvp.toUpperCase(),
          displayName: NutrientTypeValueEnums.Dvp.toUpperCase(),
        },
        {
          field: NutrientTypeValueEnums.Oep.toUpperCase(),
          displayName: NutrientTypeValueEnums.Oep.toUpperCase(),
        },
      ],
    },
    {
      header: 'Voederwaarde per kg ds',
      stylingClass: 'sticky-container two',
      columns: [
        { field: 'gDs', displayName: 'G-Ds' },
        {
          field: NutrientTypeValueEnums.Vem,
          displayName: NutrientTypeValueEnums.Vem.toUpperCase(),
        },
        {
          field: NutrientTypeValueEnums.Dvp,
          displayName: NutrientTypeValueEnums.Dvp.toUpperCase(),
        },
        {
          field: NutrientTypeValueEnums.Oep,
          displayName: NutrientTypeValueEnums.Oep.toUpperCase(),
        },
        {
          field: NutrientTypeValueEnums.Bzb,
          displayName: NutrientTypeValueEnums.Bzb.toUpperCase(),
        },
      ],
    },
    {
      header: 'Nutrienten',
      stylingClass: 'scrollable-container three',
      columns: [], // Dynamically updated
    },
  ];

  categories: Category[] = [
    { id: 1, name: 'Enkelvoudig droog' },
    { id: 2, name: 'Enkelvoudig vochtig' },
    { id: 3, name: 'Mineralen' },
    { id: 4, name: 'Standaard mengvoeders' },
    { id: 5, name: 'Standaard ruwvoeders' },
  ];
  removeFeedTypeModalHeader: string = 'Voedingssoort verwijderen uit ratsoen';

  @ViewChild(GenericTable) genericTable!: GenericTable;

  toggleCollapse(): void {
    this.isCollapsed = !this.isCollapsed;
  }
  constructor(
    private basalRationService: BasalRationService,
    private notificationService: NotificationService
  ) {}
  footerValues: Record<string, string | number> = {};

  ngOnInit(): void {
    this.initializeData();
  }

  ngAfterViewInit(): void {
    if (this.genericTable) {
      console.log('genericTable is initialized.');
    } else {
      console.warn('genericTable is not initialized.');
    }
  }

  private async initializeData(): Promise<void> {
    try {
      this.nutrients = await this.basalRationService.loadAllNutrientsTypes();

      const nutrientSection = this.basalrationSections.find(
        (section) => section.header === 'Nutrienten'
      );
      if (nutrientSection) {
        nutrientSection.columns = this.nutrients.map((nutrient) => ({
          field: nutrient.field,
          displayName: nutrient.displayName,
        }));
      }
    } catch (error) {
      console.error('Error fetching nutrients:', error);
    }
  }

  async updateRationData(rationId: number): Promise<void> {
    await this.basalRationService.updateRationData(
      rationId,
      this.processBasalRations.bind(this)
    );
  }

  processBasalRations = async (data: Ration): Promise<boolean> => {
    if (!data) {
      console.warn('No data found in the ration data.');
      return Promise.resolve(false);
    }
    if (!data.feedTypes || data.feedTypes.length === 0) {
      console.warn('No basal feeds found in the ration data.');
      this.dataSource.data = [];
      this.footerValues = {};
      this.notificationService.showError('Basisrantsoen heeft nog geen data');
      this.basalRationService.updateBasalRationTotal({ values: [] });
      return Promise.resolve(true);
    }
    this.genericTable.isLoading = true;
    this.dataSource.data = data.feedTypes.map((feed) => ({
      feedTypeId: feed.feedType.id,
      rationId: data.id,
      feedTypeName: feed.feedType.name,
      kg: feed.kgAmount,
      kgDs: feed.gAmount,
      dsProcent: feed.feedType.dsProcent,
      gDs: feed.feedType.dsProcent * 10,
      ...this.basalRationService.mapNutrientsToColumns(feed.feedType.nutrients),
    }));

    for (const row of this.dataSource.data) {
      try {
        await this.recalculateRow(row, '');
      } catch (error) {
        console.error('Error recalculating row:', error);
        return false;
      }
    }

    this.footerValues = {
      kg: data.rationTotalKg,
      kgDs: data.rationTotalKgDs,
      gDs: data.dsProcentWeightedSum,
    };

    data.averageTotalFeedTypes.forEach((avg) => {
      this.footerValues[avg.key] = avg.value;
      if (this.savedTotal.values.find((total) => total.field === avg.key)) {
        this.savedTotal.values.find((total) => total.field === avg.key)!.value =
          avg.value;
      } else {
        this.savedTotal.values.push({ field: avg.key, value: avg.value });
      }
    });

    this.basalRationService.updateBasalRationTotal(this.savedTotal);
    const mergedNutrients = this.basalRationService.mergeNutrientLists(
      this.nutrients,
      data.averageTotalFeedTypes.map((nutrient) => ({
        field: nutrient.key,
        displayName: nutrient.key,
      }))
    );

    const nutrientSection = this.basalrationSections.find(
      (section) => section.header === 'Nutrienten'
    );
    if (nutrientSection) {
      nutrientSection.columns = mergedNutrients;
    }

    this.calculateFooterTotals();
    return Promise.resolve(true);
  };

  recalculateRow = async (row: any, field: string): Promise<boolean> => {
    if (this.genericTable) {
      this.genericTable.errorMessage = '';
    }

    try {
      await this.basalRationService.recalculateRow(
        row,
        field,
        this.calculateFooterTotals.bind(this)
      );
      return true;
    } catch (error: any) {
      console.error('Error recalculating row:', error);

      if (this.genericTable) {
        this.genericTable.errorMessage = error.message;
      }

      return false;
    }
  };
  renderFooterCell(field: string): string | number {
    return this.footerValues[field] || '0';
  }

  onCellChange = async (event: {
    row: FlattenedRationFeedType;
    field: string;
  }): Promise<void> => {
    const row = event.row;
    this.genericTable.isLoading = true;
    try {
      const isValid = await this.recalculateRow(row, event.field);

      if (isValid) {
        await this.refreshTable();
        await this.basalRationService.updateRationRow(
          row,
          this.processBasalRations.bind(this)
        );
        this.notificationService.showSuccess(
          'Basisrantsoen succesvol bijgewerkt!'
        );
      } else {
        console.warn('Recalculation failed. Skipping update.');
      }
    } catch (error: any) {
      // Log errors and update the error message in the table
      console.error('Error processing cell change:', error);
      if (this.genericTable) {
        this.genericTable.errorMessage = error.message;
      }
      this.genericTable.isLoading = false;
    }
  };

  private refreshTable = async (): Promise<void> => {
    this.dataSource.data = [...this.dataSource.data];
    return Promise.resolve();
  };

  private calculateFooterTotals(): void {
    this.genericTable.isLoading = false;
    const nutrientColumns = [
      NutrientTypeValueEnums.Vem.toUpperCase(),
      NutrientTypeValueEnums.Dvp.toUpperCase(),
      NutrientTypeValueEnums.Oep.toUpperCase(),
    ];

    nutrientColumns.forEach((column) => {
      this.footerValues[column] = this.dataSource.data.reduce((sum, row) => {
        return sum + (row[column] || 0);
      }, 0);
    });
  }
  // Generic function to calculate nutritional values

  resolveFooterLabel(field: string): string | null {
    if (field === 'feedTypeName') {
      return 'Totaal basisrantsoen';
    }
    return null;
  }
  onSearchInputChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.filterValue = target.value;
  }
  handleFeedtypeDeleted({ feedTypeId }: RemoveFeedTypeModalObject): void {
    const rationId = this.dataSource.data[0]?.rationId;

    if (!rationId) {
      console.error('Ration ID is missing.');
      return;
    }

    this.basalRationService
      .removeBasalRationFeedType(rationId, feedTypeId, false)
      .subscribe({
        next: async () => {
          this.notificationService.showSuccess(
            'Voersoort succesvol verwijderd!'
          );

          await this.updateRationData(rationId);
        },
        error: (error) => {
          console.error('Error deleting feed type:', error);
          this.notificationService.showError(
            'Er is een fout opgetreden bij het verwijderen van de voersoort.'
          );
        },
      });
  }
}
