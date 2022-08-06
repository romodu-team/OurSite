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
import { FeatureComponent } from './pages/feature/feature.component';
import { Card6Component } from './shared/card6/card6.component';
import { Card7Component } from './shared/card7/card7.component';
import { AboutUsComponent } from './pages/about-us/about-us.component';


import { DemoMainComponent } from './pages/demo/demo-main/demo-main.component';
import { Card4Component } from './shared/card4/card4.component';

import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatExpansionModule} from '@angular/material/expansion';
import { Slider1Component } from './shared/slider1/slider1.component';
import {MatDialogModule} from '@angular/material/dialog';
import { ContactUsComponent } from './pages/contact-us/contact-us.component';
import { Dialog1Component } from './pages/contact-us/dialog1/dialog1.component';
import { Dialog2Component } from './pages/contact-us/dialog2/dialog2.component';


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
    FeatureComponent,
    Card6Component,
    Card7Component,
    AboutUsComponent,
    Card7Component,

    DemoMainComponent,
    Card4Component,
    Slider1Component,
    ContactUsComponent,
    Dialog1Component,
    Dialog2Component,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
