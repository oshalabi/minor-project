import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { GenericTable } from '../generic-table/generic-table.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Advice, ComparedTotalRow, Cow, TableSections } from '../../types';
import { MatTableDataSource } from '@angular/material/table';
import { MappedTotals } from '../../types';
import { NgClass, NgIf } from '@angular/common';
import { CowsOverviewService } from './cowsOverview.service';
import { BasalRationService } from '../basalration/basalration.service';
import { EnergyFoodService } from '../energy-food/energy-food.service';
import {
  combineLatest,
  delay,
  filter,
  finalize,
  retryWhen,
  Subscription,
  take,
} from 'rxjs';
import { NotificationService } from '../notification/notification.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cows-overview',
  standalone: true,
  imports: [GenericTable, MatFormFieldModule, NgClass, FormsModule, NgIf],
  templateUrl: './cowsOverview.component.html',
  styleUrl: './cowsOverview.component.css',
})
export class CowsOverviewComponent {
  protected readonly GenericTable = GenericTable;
  filterValue: string = '';
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  isCollapsed = false;

  @Input() isAllCows: boolean = false;
  @Input() tableId: string = '';
  cowsOverviewSections: TableSections[] = [];
  totals: MappedTotals = { values: [] };
  isRecalculated: boolean = false;
  isCowGroupingComparisonDone: boolean = false;
  isCowsComparisonDone: boolean = false;
  isBasalRationsSubscribed: boolean = false;
  isEnergyFoodsSubscribed: boolean = false;
  showAttentions: boolean = false;

  dataSourceCowGroupingDataChachedCompared: any[] = [];
  dataSourceCowGroupingDataChached: any[] = [];

  dataSourceCowsDataChachedCompared: any[] = [];
  dataSourceCowDataChached: any[] = [];
  @ViewChild(GenericTable) genericTable!: GenericTable;

  private subscriptions = new Subscription();
  constructor(
    private cowsService: CowsOverviewService,
    private changeDetector: ChangeDetectorRef,
    private basalRationService: BasalRationService,
    private energyFoodService: EnergyFoodService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.updateCowsSections();
    this.initializeData();
    this.resetFlags();

    this.observeData(async (basalRation, energyFood) => {
      await this.updateRows(basalRation, energyFood);
    });
  }

  async onShowAttentionsChange(event: Event): Promise<void> {
    if (this.showAttentions) {
      await this.filterForAttentions();
    } else {
      this.clearAttentionFilter();
    }
  }

  private clearAttentionFilter(): void {
    this.showAttentions = false;
    if (!this.isAllCows) {
      if (this.dataSourceCowGroupingDataChached?.length > 0) {
        this.dataSource.data = [...this.dataSourceCowGroupingDataChached];
      }
    } else {
      if (this.dataSourceCowDataChached.length > 0) {
        this.dataSource.data = [...this.dataSourceCowDataChached];
      }
    }

    this.changeDetector.detectChanges();
  }

  private async filterForAttentions(): Promise<void> {
    const data = this.dataSource.data;
    if (!this.isAllCows) {
      this.dataSourceCowGroupingDataChached = [...data];
      if (this.dataSourceCowGroupingDataChachedCompared?.length > 0) {
        this.processCowGroupingData(
          this.dataSourceCowGroupingDataChachedCompared
        );
      } else if (!this.isCowGroupingComparisonDone && data && data.length > 0) {
        await this.compareNorms(data);
      } else if (!this.isCowGroupingComparisonDone) {
        console.warn('Data source is empty or comparison already done.');
      }
    } else {
      this.dataSourceCowDataChached = [...data];
      if (this.dataSourceCowsDataChachedCompared?.length > 0) {
        this.processCowsData(this.dataSourceCowsDataChachedCompared);
      } else if (!this.isCowsComparisonDone && data && data.length > 0) {
        await this.compareNorms(data);
      } else if (!this.isCowsComparisonDone) {
        console.warn('Data source is empty or comparison already done.');
      }
    }

    this.changeDetector.detectChanges();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
  private observeData(
    callback: (basalRation: any, energyFood: any) => Promise<void>
  ): void {
    const basalRation$ = this.basalRationService.basalRationsTotal$;
    const energyFood$ = this.energyFoodService.energyFoodsValues$;

    this.subscriptions.add(
      combineLatest([basalRation$, energyFood$])
        .pipe(
          retryWhen((errors) =>
            errors.pipe(
              delay(50),
              take(3),
              finalize(() => {
                basalRation$.pipe(take(3)).subscribe((basalRation) => {
                  if (!basalRation?.values?.length) {
                    this.notificationService.showError(
                      'No data in basisrantsoen.'
                    );
                  }
                });

                energyFood$.pipe(take(3)).subscribe((energyFood) => {
                  if (!energyFood?.length) {
                    this.notificationService.showError(
                      'No data in krachtvoer.'
                    );
                  }
                });
              })
            )
          ),
          filter(([basalRation, energyFood]) => {
            const isBasalValid = basalRation?.values?.length > 0;
            const isEnergyValid = energyFood?.length > 0;

            return isBasalValid && isEnergyValid;
          })
        )
        .subscribe({
          next: async ([basalRation, energyFood]) => {
            this.isBasalRationsSubscribed = true;
            this.isEnergyFoodsSubscribed = true;

            // Pass the valid data to the callback
            await callback(basalRation, energyFood);
          },
          error: (err) => {
            console.error(
              'Error in observeData or retry limit reached. Observables may not have emitted valid data:',
              err
            );
          },
        })
    );
  }


  private async updateRows(basalRation: any, energyFood: any): Promise<void> {
    if (!this.isBasalRationsSubscribed || !this.isEnergyFoodsSubscribed) return;
    if (!this.cowsService.isDataSourceNotEmpty(this.dataSource.data)) return;

    const data = this.dataSource.data;

    if (!data || !data.length) return;
    if (this.isRecalculated) {
      return;
    }

    for (const row of data) {
      await this.recalculateRow(row, basalRation, energyFood);
    }

    if(this.showAttentions) {
      await this.compareNorms(data);
    }
    this.resetFlags();
  }

  private resetFlags(): void {
    this.isRecalculated = false;
    this.isCowGroupingComparisonDone = false;
    this.isCowsComparisonDone = false;
  }

  private async initializeData(): Promise<void> {
    try {
      await this.cowsService.initializeNutrients(this.cowsOverviewSections);
      await this.updateCowsData();
    } catch (error) {
      console.error('Initialization error:', error);
      this.notificationService.showError('Initialization failed.');
    }
  }
  get title(): string {
    return this.isAllCows ? 'Alle koeien' : 'Krachtvoeroverzicht';
  }
  private updateCowsSections(): void {
    this.cowsOverviewSections = [
      !this.isAllCows
        ? {
          header: '',
          stylingClass: 'sticky-container',
          columns: [
            { field: 'totalCows', displayName: 'Aantal' },
            { field: 'name', displayName: 'Naam' },
            { field: 'days', displayName: 'Dgn' },
          ],
        }
        : {
          header: '',
          stylingClass: 'sticky-container',
          columns: [
            { field: 'lactationId', displayName: 'Lac…' },
            { field: 'id', displayName: 'Nr.' },
            { field: 'name', displayName: 'Naam' },
            { field: 'days', displayName: 'Dgn' },
          ],
        },
      {
        header: 'Advies',
        stylingClass: 'sticky-container',
        columns: [
          { field: 'advice1', displayName: '1' },
          { field: 'advice2', displayName: '2' },
          { field: 'advice3', displayName: '3' },
          { field: 'advice4', displayName: '4' },
        ],
      },
      {
        header: 'Dagproductie',
        stylingClass: 'sticky-container one',
        columns: [
          { field: 'milk', displayName: 'Melk' },
          { field: 'fat', displayName: 'Vet' },
          { field: 'protein', displayName: 'Eiwit' },
        ],
      },
      {
        header: 'Opname (kg ds)',
        stylingClass: 'sticky-container two',
        columns: [
          { field: 'ruwv', displayName: 'Ruwv.' },
          { field: 'total', displayName: 'Totaal' },
          { field: 'rv', displayName: '%RV.' },
        ],
      },
      {
        header: 'Nutrienten',
        stylingClass: 'scrollable-container three',
        columns: [],
      },
    ];
  }

  private async updateCowsData(): Promise<void> {
    try {
      if (this.isAllCows) {
        await this.cowsService.getCowsData((data) => {
          this.processCowsData(data);
          this.changeDetector.detectChanges();
          this.updatecowOverviewAdvices(data);
          return Promise.resolve(true);
        });
      } else {
        this.cowsService.getCowGrouping(1).subscribe((data) => {
          this.processCowGroupingData(data);
          this.changeDetector.detectChanges();
        });
      }
    } catch (error) {
      console.error('Error updating cows data:', error);
      this.notificationService.showError('Failed to update cows data.');
    }
  }

  processCowGroupingData = async (data: any): Promise<boolean> => {
    return this.processData(
      data,
      (compared: ComparedTotalRow, index: number) => ({
        id: `group-${index}`,
        totalCows: compared.totalCows,
        name: compared.name,
        days: compared.days.toFixed(2),
        milk: compared.milk.toFixed(2),
        fat: compared.fat.toFixed(2),
        protein: compared.protein.toFixed(2),
        total: compared.total.toFixed(2),
        rv: compared.rv.toFixed(2),
        advice1: compared.advices?.[0]?.value ?? 0,
        advice2: compared.advices?.[1]?.value ?? 0,
        advice3: compared.advices?.[2]?.value ?? 0,
        advice4: compared.advices?.[3]?.value ?? 0,
        ruwv: ((compared.total * compared.rv) / 100).toFixed(2),
        group: compared.group,
        totals: compared.totals,
      })
    );
  };

  processCowsData = async (data: any): Promise<boolean> => {
    if (this.isCowsComparisonDone) {
      this.dataSource.data = this.dataSource.data.map((existingRow: any) => {
        const updatedRow = data.find((row: any) => row.id === existingRow.id);
        if (updatedRow) {
          return {
            ...existingRow,
            totals: updatedRow.totals,
          };
        }
        return existingRow;
      });
      this.changeDetector.detectChanges();
      return Promise.resolve(true);
    }

    return this.processData(data, (cow: Cow) => ({
      id: cow.id,
      name: cow.name,
      lactationId: cow.lactationId,
      lactationName: cow.lactationName,
      days: cow.days,
      milk: cow.milk,
      fat: cow.fat,
      protein: cow.protein,
      total: cow.total,
      rv: cow.rv,
      advice1: cow.advices?.[0]?.value ?? 0,
      advice2: cow.advices?.[1]?.value ?? 0,
      advice3: cow.advices?.[2]?.value ?? 0,
      advice4: cow.advices?.[3]?.value ?? 0,
      ruwv: ((cow.total * cow.rv) / 100).toFixed(2),
      totals: [],
    }));
  };

  updatecowOverviewAdvices(data: Cow[]): void {
    if (!data) {
      this.notificationService.showError(
        'Koeien hebben nog geen advices.'
      );
      return;
    }

    const adviceSums = [0, 0, 0, 0];
    const adviceCounts = [0, 0, 0, 0];

    data.forEach((cow) => {
      cow.advices?.forEach((advice, index) => {
        if (index < 4) {
          adviceSums[index] += advice.value;
          adviceCounts[index]++;
        }
      });
    });

    // Calculate the averages for each advice
    const averages = adviceSums.map((sum, index) =>
      adviceCounts[index] > 0 ? sum / adviceCounts[index] : 0
    );

    // Map the averages to advice fields
    const advices: Advice[] = averages.map((average, index) => ({
      field: `advice${index + 1}`,
      value: average,
    }));
    this.cowsService.setcowOverviewAdvices(advices);
  }

  private async processData<T>(
    data: T[],
    mapFunction: (item: T, index: number) => any
  ): Promise<boolean> {
    if (!data) {
      console.warn('No data response from the server.');
      this.dataSource.data = [];
      return Promise.resolve(false);
    }

    if (!data.length || data.length === 0) {
      console.warn('No data found.');
      this.dataSource.data = [];
      return Promise.resolve(true);
    }

    this.genericTable.isLoading = true;
    this.dataSource.data = data.map(mapFunction);
    this.genericTable.isLoading = false;

    return Promise.resolve(true);
  }

  private async compareNorms(data: any): Promise<void> {
    if (!this.isAllCows) {
      if (!this.isCowGroupingComparisonDone) {
        try {
          const mappedData = await this.cowsService.mapCowGroupingToCompare(
            data
          );
          this.cowsService.compareNormsCowGrouping(mappedData).subscribe(
            (responseData) => {
              this.isCowGroupingComparisonDone = true;
              this.processCowGroupingData(responseData);
              this.dataSourceCowGroupingDataChachedCompared = [...responseData];
            },
            (error) => {
              console.error('Error from compareNorms:', error);
            }
          );
        } catch (error) {
          console.error('Error mapping data for comparison:', error);
          this.notificationService.showError('Failed to compare norms.');
        }
      }
    } else {
      if (!this.isCowsComparisonDone) {
        try {
          const mappedData = await this.cowsService.mapCowsToCompare(data);
          this.cowsService.compareNormsAllCow(mappedData).subscribe(
            (responseData) => {
              this.isCowsComparisonDone = true;
              this.processCowsData(responseData);
              this.dataSourceCowsDataChachedCompared = this.dataSource.data.map(
                (existingRow: any) => {
                  const updatedRow = responseData.find(
                    (row: any) => row.id === existingRow.id
                  );
                  if (updatedRow) {
                    return {
                      ...existingRow,
                      totals: updatedRow.totals,
                    };
                  }
                  return existingRow;
                }
              );
            },
            (error) => {
              console.error('Error from compareNorms:', error);
            }
          );
        } catch (error) {
          console.error('Error mapping data for comparison:', error);
          this.notificationService.showError('Failed to compare norms.');
        }
      }
    }
  }
  toggleCollapse(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  private async recalculateRow(
    row: any,
    basalRation: any,
    energyFood: any
  ): Promise<boolean> {
    try {
      await this.calculateNutrientsValues(row, basalRation, energyFood);
      this.isRecalculated = true;
      return true;
    } catch (error: any) {
      console.error('Error recalculating row:', error);
      this.notificationService.showError(
        'Er is een fout opgetreden bij het berekenen van de nutrienten.'
      );
      return false;
    }
  }

  private async calculateNutrientsValues(
    row: any,
    basalRation: any,
    energyFood: any
  ): Promise<void> {
    try {
      const nutrientsBasalRationValues =
        await this.cowsService.calculateNutrientsBasalValues(row, basalRation);
      const nutrientsEnergyFoodValues =
        await this.cowsService.energyFoodsValues(row, energyFood);

      if (
        !nutrientsBasalRationValues?.values &&
        !nutrientsEnergyFoodValues?.values
      ) {
        return;
      }

      const totals = nutrientsBasalRationValues.values.map((basalNutrient) => {
        const energyNutrient = nutrientsEnergyFoodValues.values.find(
          (n) => n.field.toLowerCase() === basalNutrient.field.toLowerCase()
        );

        const basalValue = Number(basalNutrient.value) || 0;
        const energyValue = Number(energyNutrient?.value) || 0;
        const total = Number(row.total) || 1;

        const calculatedValue = ((basalValue - energyValue) / total).toFixed(2);

        return {
          field: basalNutrient.field,
          value: parseFloat(calculatedValue),
        };
      });
      this.dataSource.data = this.dataSource.data.map((dataRow: any) => {
        if (dataRow.id === row.id) {
          dataRow.totals = totals;
        }
        return dataRow;
      });

      this.changeDetector.detectChanges();
    } catch (error) {
      this.notificationService.showError(
        'Er is een fout opgetreden bij het berekenen van de nutrienten.'
      );
      console.error('Error calculating nutrient values:', error);
    }
  }

  onSearchInputChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.filterValue = target.value;
  }
}
