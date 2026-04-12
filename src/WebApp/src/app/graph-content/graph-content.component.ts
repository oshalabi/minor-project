import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { Category } from './graph-content-enum.component';
import { GraphDataService } from './graph-content.service';
import { Subscription } from 'rxjs';
import { RationService } from '../ration/ration.service';

type AxisTitle = 'Melkproductie (liters)' | 'Krachtvoer (kg)' | 'Dagen' | 'kg';

const CHART_TYPES = {
  Opstarttabel: 'Opstarttabel',
  VoertabelMelk: 'VoertabelMelk',
} as const;

type GraphType = keyof typeof CHART_TYPES;

@Component({
  selector: 'app-graph-content',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './graph-content.component.html',
  styleUrls: ['./graph-content.component.css'],
})
export class GraphContentComponent implements AfterViewInit, OnDestroy {
  Category = Category;
  selectedCategory: Category = Category.OudereDieren;
  chartInstances: { [key in GraphType]: Chart | null } = {
    [CHART_TYPES.Opstarttabel]: null,
    [CHART_TYPES.VoertabelMelk]: null,
  };
  private rationId: number | null = null;
  private rationIdSubscription: Subscription = new Subscription();
  private emptyGraphData = { melkproductieVEM: 0, energyFoodAmount: 0 };

  categoryList = [
    { value: Category.OudereDieren, label: 'Oudere dieren' },
    { value: Category.TweedeKalfs, label: '2e Kalfs' },
    { value: Category.Vaarzen, label: 'Vaarzen' },
  ];

  constructor(
    private graphDataService: GraphDataService,
    private rationService: RationService
  ) {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      Chart.register(...registerables);

      this.rationIdSubscription = this.graphDataService.rationId$.subscribe((rationId) => {
        this.rationId = rationId;

        if (this.rationId) {
          this.updateGraph(CHART_TYPES.Opstarttabel);
          this.updateGraph(CHART_TYPES.VoertabelMelk);
        }
      });

      this.initializeCharts();
    }, 0);
  }

  ngOnDestroy(): void {
    this.destroyAllCharts();
    this.rationIdSubscription.unsubscribe();
  }

  private initializeCharts(): void {
    if (!this.chartInstances[CHART_TYPES.Opstarttabel]) {
      this.createChart(CHART_TYPES.Opstarttabel, false);
    }
    if (!this.chartInstances[CHART_TYPES.VoertabelMelk]) {
      this.createChart(CHART_TYPES.VoertabelMelk, true);
    }
  }

  private createChart(type: GraphType, isYAxisRight: boolean): void {
    const canvasElement = this.getCanvasElement(type);

    if (canvasElement && !this.chartInstances[type]) {
      this.chartInstances[type] = this.createChartInstance(canvasElement, type, isYAxisRight);
    }
  }

  private getCanvasElement(type: GraphType): HTMLCanvasElement | null {
    const canvasId = `chart-${type.toLowerCase().replace(/\s+/g, '-')}`;
    return document.getElementById(canvasId) as HTMLCanvasElement;
  }

  private createChartInstance(
    canvasElement: HTMLCanvasElement,
    type: GraphType,
    isYAxisRight: boolean
  ): Chart {
    const axisTitles: { [key in GraphType]: { x: AxisTitle; y: AxisTitle } } = {
      [CHART_TYPES.VoertabelMelk]: {
        x: 'Melkproductie (liters)',
        y: 'Krachtvoer (kg)',
      },
      [CHART_TYPES.Opstarttabel]: {
        x: 'Dagen',
        y: 'kg',
      },
    };

    const xAxisTitle = axisTitles[type].x;
    const yAxisTitle = axisTitles[type].y;

    const xTicksConfig =
      type === CHART_TYPES.Opstarttabel
        ? {
            stepSize: 5,
            autoSkip: false,
            callback: (tickValue: any): string | null => {
              if (typeof tickValue === 'number' && tickValue % 5 === 0) {
                return tickValue.toString();
              }
              return null;
            },
          }
        : {};

    return new Chart(canvasElement, {
      type: 'line',
      data: { labels: [], datasets: [] },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'top' },
        },
        scales: {
          x: {
            title: { display: true, text: xAxisTitle },
            ticks: {
              ...xTicksConfig,
            },
            min: type === CHART_TYPES.Opstarttabel ? 0 : undefined,
            max: type === CHART_TYPES.Opstarttabel ? 60 : undefined,
            type: 'linear',
            reverse: type === CHART_TYPES.VoertabelMelk ? true : false,
          },
          y: {
            title: { display: true, text: yAxisTitle },
            beginAtZero: true,
            position: isYAxisRight ? 'right' : 'left', // Dynamisch instellen
          },
        },
      },
    });
  }

  private destroyAllCharts(): void {
    Object.keys(this.chartInstances).forEach((key) => {
      const graphKey = key as GraphType;
      if (this.chartInstances[graphKey]) {
        this.chartInstances[graphKey]!.destroy();
        this.chartInstances[graphKey] = null;
      }
    });
  }

  async updateGraph(type: GraphType): Promise<void> {
    if (!this.rationId) {
      return;
    }

    const backendData = await this.fetchBackendData(this.selectedCategory);

    if (this.chartInstances[type]) {
      this.chartInstances[type]!.data = this.getChartData(type, backendData);
      this.chartInstances[type]!.update();
    }
  }

  private async fetchBackendData(category: Category): Promise<{ melkproductieVEM: number; energyFoodAmount: number }> {
    const parityTypeValue = this.getParityTypeValue(category);

    if (!this.rationId) {
      console.error('Ration ID is not available for fetching data.');
      return this.emptyGraphData;
    }

    try {
      const livestockData = await this.rationService
        .GetLivestockProperty(this.rationId)
        .toPromise();

      const energyFoodAmount = 10;
      const basalRationVEMBasicAmount = 16085;
      const basalRationDVPAmount = 10;

      const graphDataResponse = await this.graphDataService
        .getGraphData(
          energyFoodAmount,
          basalRationDVPAmount,
          basalRationVEMBasicAmount,
          livestockData
        )
        .toPromise();

      return graphDataResponse ?? this.emptyGraphData;
    } catch (error) {
      console.error('Error fetching backend data:', error);
      return this.emptyGraphData;
    }
  }


  getChartData(type: string, backendData?: { melkproductieVEM: number; energyFoodAmount: number }): any {
    if (type === 'VoertabelMelk' && backendData) {
      const { melkproductieVEM, energyFoodAmount } = backendData;
      const step = 0.5;
      const incrementPerKg = 2;
      const maxKg = energyFoodAmount;

      const adjustedMelkproductieVEM =
        this.selectedCategory === Category.Vaarzen
          ? melkproductieVEM * 0.8
          : melkproductieVEM;

      const dataPoints: { x: number; y: number }[] = [];
      for (let i = 0; i <= maxKg; i += step) {
        const kg = i.toFixed(1);
        const melkproductie = adjustedMelkproductieVEM + i * incrementPerKg;
        dataPoints.push({ x: melkproductie, y: parseFloat(kg) });
      }

      dataPoints.sort((a, b) => b.x - a.x);

      return {
        labels: dataPoints.map((point) => `${point.x.toFixed(1)}`),
        datasets: [
          {
            label: `${this.selectedCategory} - Melkproductie`,
            data: dataPoints.map((point) => point.y),
            borderColor: 'rgba(0, 124, 40, 1)',
            pointBackgroundColor: 'rgba(0, 124, 40, 1)',
            pointBorderColor: 'white',
            pointBorderWidth: 2,
            backgroundColor: 'rgba(0, 124, 40, 0.2)',
            fill: true,
            tension: 0.4,
          },
        ],
      };
    }

    // Opstarttabel logic
    if (type === 'Opstarttabel') {
      const totalDays = 60;
      const step = 5;

      const dataPoints: { x: number; y: number }[] = [];
      for (let day = 0; day <= totalDays; day += step) {
        const kg = (day / 6).toFixed(1);
        dataPoints.push({ x: day, y: parseFloat(kg) });
      }

      return {
        labels: dataPoints.map((point) => `${point.x}`),
        datasets: [
          {
            label: 'Gewichtstoename',
            data: dataPoints.map((point) => point.y),
            borderColor: 'rgba(0, 124, 40, 1)',
            backgroundColor: 'rgba(0, 124, 40, 0.2)',
            pointBackgroundColor: 'rgba(0, 124, 40, 1)',
            pointBorderColor: 'white',
            pointBorderWidth: 2,
            fill: true,
            tension: 0.4,
          },
        ],
      };
    }

    return { labels: [], datasets: [] };
  }

  selectCategory(category: Category): void {
    this.selectedCategory = category;
    this.updateGraph(CHART_TYPES.Opstarttabel);
    this.updateGraph(CHART_TYPES.VoertabelMelk);
  }

  private getParityTypeValue(category: Category): number {
    switch (category) {
      case Category.Vaarzen:
        return 3;
      case Category.TweedeKalfs:
        return 4;
      case Category.OudereDieren:
        return 5;
      default:
        return 5;
    }
  }
}
