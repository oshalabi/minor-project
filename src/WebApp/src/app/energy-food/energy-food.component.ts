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
  Advice,
} from '../../types';
import { NgClass, NgIf } from '@angular/common';
import { EnergyFoodService } from './energy-food.service';
import { EnergyFoodModalComponent } from '../modals/energy-food-modal/energy-food-modal.component';
import { EnergyFeedSettingsModalComponent } from '../modals/energy-feed-settings-modal/energy-feed-settings-modal.component';
import { NotificationService } from '../notification/notification.service';
import { delay, filter, finalize, retryWhen, Subscription, take } from 'rxjs';
import { CowsOverviewService } from '../cowsOverview/cowsOverview.service';
import { RationService } from '../ration/ration.service';

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
    EnergyFeedSettingsModalComponent,
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
        { field: 'kgDs', displayName: 'kg ds', editable: false },
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

  private subscriptions = new Subscription();
  cowOverviewAdvices: Advice[] = [];
  constructor(
    private energyFoodService: EnergyFoodService,
    private notificationService: NotificationService,
    private cowsOverViewService: CowsOverviewService,
    private rationService: RationService
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
    this.observeData();
  }

  private observeData(): void {
    const cowOverview$ = this.cowsOverViewService.cowOverviewAdvices$;

    this.subscriptions.add(
      cowOverview$
        .pipe(
          retryWhen((errors) =>
            errors.pipe(
              delay(50), // Delay between retries
              take(3), // Retry up to 3 times
              finalize(() => {
                cowOverview$.subscribe((cowOverview) => {
                  if (!cowOverview?.length) {
                    this.notificationService.showError(
                      'Geen advices gevonden.'
                    );
                  }
                });
              })
            )
          ),
          filter((cowOverview) => {
            const isCowOverviewValid = cowOverview?.length > 0;
            return isCowOverviewValid;
          })
        )
        .subscribe({
          next: async (cowOverview) => {
            try {
              this.cowOverviewAdvices = cowOverview;
            } catch (err) {
              console.error('Error in callback execution:', err);
            }
          },
          error: (err) => {
            console.error(
              'Error during data observation or retry limit reached:',
              err
            );
          },
        })
    );
  }

  async updateRationData(rationId: number): Promise<void> {
    if (!rationId) {
      console.warn('Invalid ration ID provided.');
      return Promise.reject('Invalid ration ID.');
    }

    this.rationService.getRationById(rationId, true).subscribe({
      next: async (data: Ration) => {
        if (!data) {
          console.warn('No data found for the provided ration ID.');
          return Promise.reject('No data found.');
        }

        try {
          await this.processEnergyFoods(data);
        } catch (error) {
          console.error('Error processing basal rations:', error);
        }
      },
      error: (err) => {
        console.error('Error fetching basal rations by ID:', err);
      },
    });
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

      this.notificationService.showError('krachtvoer heeft nog geen data');
      this.energyFoodService.setEnergyFoodsNutrientValues([]);

      return Promise.resolve(true);
    }

    for (const [index, feed] of data.feedTypes.entries()) {
      if (!feed.feedType) {
        console.warn('No feed type found for the feed.');
        return Promise.resolve(false);
      } else {
        const advices = this.cowOverviewAdvices;

        const adviceIndex = index % advices.length;
        const advice = advices[adviceIndex];
        const adviceValue = advice?.value || 0;

        const kgDs = parseFloat(
          ((adviceValue * feed.feedType.dsProcent) / 100).toFixed(2)
        );

        if (feed.kgAmount != kgDs || feed.gAmount != kgDs) {
          if (feed.kgAmount != kgDs || feed.gAmount != kgDs) {
            await this.energyFoodService.updateRationRow(
              {
                feedTypeId: feed.feedType.id,
                isEnergy: true,
                feedTypeKg: kgDs,
                feedTypeG: kgDs,
              },
              data.id,
              this.processEnergyFoods.bind(this)
            );
          }
        }
      }
    }
    this.dataSource.data = data.feedTypes.map((feed) => {
      return {
        feedTypeId: feed.feedType.id,
        rationId: data.id,
        feedTypeName: feed.feedType.name,
        kg: feed.kgAmount,
        kgDs: feed.gAmount,
        dsProcent: feed.feedType.dsProcent,
        gDs: feed.feedType.dsProcent * 10,
        ...this.energyFoodService.mapNutrientsToColumns(
          feed.feedType.nutrients
        ),
      };
    });

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

    const energyFoods: EnergyFoordNutrientValue[] = data.feedTypes.map(
      (feed, index) => ({
        advice: index + 1,
        nutrientsValues: feed.feedType.nutrients.map((nutrient) => ({
          field: nutrient.code,
          value: nutrient.value,
        })),
      })
    );

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

    await this.calculateFooterTotals();
    return Promise.resolve(true);
  };

  recalculateRow = async (row: any, field: string): Promise<boolean> => {
    if (this.genericTable) {
      this.genericTable.errorMessage = '';
    }

    try {
      await this.energyFoodService.recalculateRow(
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
