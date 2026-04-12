import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BasalrationComponent } from './basalration/basalration.component';
import { BrowserModule } from '@angular/platform-browser';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

@NgModule({
  declarations: [],
  imports: [
    AppComponent,
    BasalrationComponent,
    BrowserModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    SweetAlert2Module.forRoot(),
  ],
})
export class AppModule {}
