import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { HomeComponent } from './pages/home/home.component';
import { HeaderComponent } from './pages/home/header/header.component';
import { ServicesComponent } from './pages/home/services/services.component';
import { FormComponent } from './pages/home/form/form.component';
import { Card1Component } from './shared/card1/card1.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { Card2Component } from './shared/card2/card2.component';
import { Card3Component } from './shared/card3/card3.component';
import { Card5Component } from './shared/card5/card5.component';
import { DemoFormComponent } from './shared/demo-form/demo-form.component';
import { DemoDetailHeaderComponent } from './pages/demo-detail/demo-detail-header/demo-detail-header.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    ServicesComponent,
    FormComponent,
    Card1Component,
    NavbarComponent,
    FooterComponent,
    Card2Component,
    Card3Component,
    Card5Component,
    DemoFormComponent,
    DemoDetailHeaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
