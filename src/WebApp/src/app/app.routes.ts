import { Routes } from '@angular/router';
import { BasalrationComponent } from './basalration/basalration.component';

export const routes: Routes = [
  { path: 'Ration/BasalRations/:id', component: BasalrationComponent },
  { path: '**', redirectTo: '' },
];
