import { Component, Input, ViewChild } from '@angular/core';
import { GenericTable } from '../generic-table/generic-table.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AddFeedtypeModalComponent } from '../modals/add-feedtype-modal/add-feedtype-modal.component';
import { MatTableDataSource } from '@angular/material/table';
import {
  FlattenedRationFeedType,
  Ration,
  TableSections,
  NutrientTypeValueEnums,
  Category,
  RemoveFeedTypeModalObject,
  EnergyFoordNutrientValue,
} from '../../types';
import { NgClass, NgIf } from '@angular/common';
import { EnergyFoodService } from './energy-food.service';
import { EnergyFoodModalComponent } from '../modals/energy-food-modal/energy-food-modal.component';
import { EnergyFeedSettingsModalComponent } from '../modals/energy-feed-settings-modal/energy-feed-settings-modal.component';
import { NotificationService } from '../notification/notification.service';

@Component({
  selector: 'app-energy-food',
  standalone: true,
  imports: [
    GenericTable,
    MatFormFieldModule,
    AddFeedtypeModalComponent,
    NgClass,
    NgIf,
    EnergyFoodModalComponent,
    EnergyFeedSettingsModalComponent
  ],
  templateUrl: './energy-food.component.html',
  styleUrl: './energy-food.component.css',
})
export class EnergyFoodComponent {
  protected readonly GenericTable = GenericTable;
  filterValue: string = '';
  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource();
  nutrients: Array<{ field: string; displayName: string }> = [];
  isCollapsed = false;
  energyFoodSections: TableSections[] = [
    {
      header: '',
      stylingClass: 'sticky-container',
      columns: [
        { field: 'feedTypeName', displayName: 'Voersoort' },
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
    { id: 5, name: 'Standaard mengvoeders' },
  ];

  @ViewChild(GenericTable) genericTable!: GenericTable;
  removeFeedTypeModalHeader: string = 'Voeder verwijderen';
  toggleCollapse(): void {
    this.isCollapsed = !this.isCollapsed;
  }
  constructor(
    private energyFoodService: EnergyFoodService,
    private notificationService: NotificationService
  ) {}
  footerValues: Record<string, string | number> = {};
  private async initializeData(): Promise<void> {
    try {
      this.nutrients = await this.energyFoodService.loadAllNutrientsTypes();

      const nutrientSection = this.energyFoodSections.find(
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

  ngOnInit(): void {
    this.initializeData();
  }

  async updateRationData(rationId: number): Promise<void> {
    await this.energyFoodService.updateRationData(
      rationId,
      this.processEnergyFoods.bind(this)
    );
  }
  processEnergyFoods = async (data: Ration): Promise<boolean> => {
    if (!data) {
      console.warn('No data found in the ration data.');
      return Promise.resolve(false);
    }
    if (!data.feedTypes || data.feedTypes.length === 0) {
      console.warn('No energy feeds found in the ration data.');
      this.dataSource.data = [];
      this.footerValues = {};

      this.notificationService.showError("krachtvoer heeft nog geen data")
      this.energyFoodService.setEnergyFoodsNutrientValues([]);

      return Promise.resolve(true);
    }

    this.dataSource.data = data.feedTypes.map((feed) => ({
      feedTypeId: feed.feedType.id,
      rationId: data.id,
      feedTypeName: feed.feedType.name,
      kg: feed.kgAmount,
      kgDs: feed.gAmount,
      dsProcent: feed.feedType.dsProcent,
      gDs: feed.feedType.dsProcent * 10,
      ...this.energyFoodService.mapNutrientsToColumns(feed.feedType.nutrients),
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
    });

    const energyFoods: EnergyFoordNutrientValue[] = data.feedTypes.map((feed, index) => ({
      advice: index + 1,
      nutrientsValues: feed.feedType.nutrients.map((nutrient) => ({
        field: nutrient.code,
        value: nutrient.value,
      })),
    }));

    this.energyFoodService.setEnergyFoodsNutrientValues(energyFoods);

    const mergedNutrients = this.energyFoodService.mergeNutrientLists(
      this.nutrients,
      data.averageTotalFeedTypes.map((nutrient) => ({
        field: nutrient.key,
        displayName: nutrient.key,
      }))
    );

    const nutrientSection = this.energyFoodSections.find(
      (section) => section.header === 'Nutrienten'
    );
    if (nutrientSection) {
      nutrientSection.columns = mergedNutrients;
    }

    this.calculateFooterTotals();
    return Promise.resolve(true);
  };

  recalculateRow = async (row: any, field: string): Promise<boolean> => {
    // Clear previous error message
    if (this.genericTable) {
      this.genericTable.errorMessage = '';
    }

    try {
      // Attempt recalculation
      await this.energyFoodService.recalculateRow(
        row,
        field,
        this.calculateFooterTotals.bind(this)
      );
      return true; // Successful recalculation
    } catch (error: any) {
      // Log the error
      console.error('Error recalculating row:', error);

      // Set the error message based on the type of error
      if (this.genericTable) {
        this.genericTable.errorMessage = error.message;
      }

      return false; // Failed recalculation
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

    try {
      // First, attempt to recalculate the row
      const isValid = await this.recalculateRow(row, event.field);

      // If recalculation is successful, refresh the table and update the row
      if (isValid) {
        await this.refreshTable();
        await this.energyFoodService.updateRationRow(
          row,
          this.processEnergyFoods.bind(this)
        );
        this.notificationService.showSuccess(
          'Krachtvoer succesvol bijgewerkt!'
        );
      } else {
        console.warn('Recalculation failed. Skipping update.');
      }
    } catch (error: any) {
      // Handle errors from recalculation
      console.error('Error processing cell change:', error);
      this.genericTable.errorMessage = error.message;
    }
  };

  private refreshTable = async (): Promise<void> => {
    this.dataSource.data = [...this.dataSource.data];
    return Promise.resolve();
  };

  private calculateFooterTotals(): void {
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
      return 'Totaal krachtvoer';
    }
    return null;
  }
  onSearchInputChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.filterValue = target.value; // Update de zoekwaarde
  }
  handleFeedtypeDeleted({ feedTypeId }: RemoveFeedTypeModalObject): void {
    const rationId = this.dataSource.data[0]?.rationId;

    if (!rationId) {
      console.error('Ration ID is missing.');
      return;
    }

    this.energyFoodService
      .removeEnergyFoodFeedType(rationId, feedTypeId, true)
      .subscribe({
        next: async () => {
          this.notificationService.showSuccess(
            'Voersoort succesvol verwijderd!'
          );

          this.dataSource.data = this.dataSource.data.filter(
            (row) => row.feedTypeId !== feedTypeId
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
