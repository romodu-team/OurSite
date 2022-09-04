import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
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
import {MatSidenavModule} from '@angular/material/sidenav';
import { Slider1Component } from './shared/slider1/slider1.component';
import {MatDialogModule} from '@angular/material/dialog';
import { ContactUsComponent } from './pages/contact-us/contact-us.component';
import { Dialog1Component } from './pages/contact-us/dialog1/dialog1.component';
import { Dialog2Component } from './pages/contact-us/dialog2/dialog2.component';


import { Card5Component } from './shared/card5/card5.component';
import { DemoFormComponent } from './shared/demo-form/demo-form.component';
import { DemoDetailHeaderComponent } from './pages/demo/demo-detail-header/demo-detail-header.component';

import {MatIconModule} from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { UserComponent } from './pages/user/user.component';
import { AuthInterceptor } from './shared/services/auth.interceptor';
import {MatTooltipModule} from '@angular/material/tooltip';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard/admin-dashboard.component';
import { AdminManagmentComponent } from './pages/admin/admin-managment/admin-managment.component';
import { AdminManagementModalComponent } from './pages/admin/admin-managment/admin-management-modal/admin-management-modal.component';
import { AdminManagementEditComponent } from './pages/admin/admin-managment/admin-management-edit/admin-management-edit.component';
import { AdminUserProjectComponent } from './pages/admin/admin-user-project/admin-user-project.component';
import { AdminUserProjectModalComponent } from './pages/admin/admin-user-project/admin-user-project-modal/admin-user-project-modal.component';
import { AdminUserProjectNewUserComponent } from './pages/admin/admin-user-project/admin-user-project-new-user/admin-user-project-new-user.component';
import { AdminUserProjectEditUserComponent } from './pages/admin/admin-user-project/admin-user-project-edit-user/admin-user-project-edit-user.component';
import { AdminProjectProjectComponent } from './pages/admin/admin-user-project/admin-project-project/admin-project-project.component';
import { AdminProjectComponent } from './pages/admin/admin-project/admin-project.component';
import { AdminProjectModalComponent } from './pages/admin/admin-project/admin-project-modal/admin-project-modal.component';
import { AdminProjectEditComponent } from './pages/admin/admin-project/admin-project-edit/admin-project-edit.component';
import { AdminProjectNewComponent } from './pages/admin/admin-project/admin-project-new/admin-project-new.component';
import { AdminProjectFeaturesComponent } from './pages/admin/admin-project/admin-project-features/admin-project-features.component';
import { AdminWorksampleComponent } from './pages/admin/admin-worksample/admin-worksample.component';
import { AdminWorksampleCreateComponent } from './pages/admin/admin-worksample/admin-worksample-create/admin-worksample-create.component';
import { AdminWorksampleEditComponent } from './pages/admin/admin-worksample/admin-worksample-edit/admin-worksample-edit.component';
import { AdminWorksampleCategoryComponent } from './pages/admin/admin-worksample/admin-worksample-category/admin-worksample-category.component';
import { AdminWorksampleCategoryCreateeditComponent } from './pages/admin/admin-worksample/admin-worksample-category/admin-worksample-category-createedit/admin-worksample-category-createedit.component';
import { AdminCheckboxComponent } from './pages/admin/admin-checkbox/admin-checkbox.component';
import { AdminCheckboxModalComponent } from './pages/admin/admin-checkbox/admin-checkbox-modal/admin-checkbox-modal.component';
import { AdminRoleManagementComponent } from './pages/admin/admin-role-management/admin-role-management.component';
import { AdminRoleManagementModalComponent } from './pages/admin/admin-role-management/admin-role-management-modal/admin-role-management-modal.component';
import { AdminRoleManagementAccessComponent } from './pages/admin/admin-role-management/admin-role-management-access/admin-role-management-access.component';
import { AdminPaymentComponent } from './pages/admin/admin-payment/admin-payment.component';
import { AdminPaymentModalComponent } from './pages/admin/admin-payment/admin-payment-modal/admin-payment-modal.component';
import { AdminTicketComponent } from './pages/admin/admin-ticket/admin-ticket.component';
import { AdminTicketModalComponent } from './pages/admin/admin-ticket/admin-ticket-modal/admin-ticket-modal.component';
import { AdminTicketTicketComponent } from './pages/admin/admin-ticket/admin-ticket-ticket/admin-ticket-ticket.component';
import { AdminTicketPriovityComponent } from './pages/admin/admin-ticket/admin-ticket-priovity/admin-ticket-priovity.component';
import { AdminTicketPriorityModalComponent } from './pages/admin/admin-ticket/admin-ticket-priovity/admin-ticket-priority-modal/admin-ticket-priority-modal.component';
import { AdminTicketStatusComponent } from './pages/admin/admin-ticket/admin-ticket-status/admin-ticket-status.component';
import { AdminTicketStatusModalComponent } from './pages/admin/admin-ticket/admin-ticket-status/admin-ticket-status-modal/admin-ticket-status-modal.component';
import { AdminCounselingComponent } from './pages/admin/admin-counseling/admin-counseling.component';
import { AdminCounselingModalComponent } from './pages/admin/admin-counseling/admin-counseling-modal/admin-counseling-modal.component';
import { AdminCounselingCounselComponent } from './pages/admin/admin-counseling/admin-counseling-counsel/admin-counseling-counsel.component';
import { AdminCountactComponent } from './pages/admin/admin-countact/admin-countact.component';
import { AdminCountactCountactComponent } from './pages/admin/admin-countact/admin-countact-countact/admin-countact-countact.component';
import { AdminCountactNewComponent } from './pages/admin/admin-countact/admin-countact-new/admin-countact-new.component';
import { UesrDashboardComponent } from './pages/user/uesr-dashboard/uesr-dashboard.component';
import { UesrProfileComponent } from './pages/user/uesr-profile/uesr-profile.component';
import { UesrTicketComponent } from './pages/user/uesr-ticket/uesr-ticket.component';
import { UserTicketModalComponent } from './pages/user/uesr-ticket/user-ticket-modal/user-ticket-modal.component';
import { UserTicketTicketComponent } from './pages/user/uesr-ticket/user-ticket-ticket/user-ticket-ticket.component';
import { UesrProjectComponent } from './pages/user/uesr-project/uesr-project.component';
import { UserProjectProjectComponent } from './pages/user/uesr-project/user-project-project/user-project-project.component';
import { UserProjectNewComponent } from './pages/user/uesr-project/user-project-new/user-project-new.component';
import { Card8Component } from './shared/card8/card8.component';
import { Card9Component } from './shared/card9/card9.component';
import { Card10Component } from './shared/card10/card10.component';
import { Card11Component } from './shared/card11/card11.component';
import { Card12Component } from './shared/card12/card12.component';
import { Card13Component } from './shared/card13/card13.component';
import { AdminuserHeaderComponent } from './shared/adminuser-header/adminuser-header.component';
import { AdminuserNavbarComponent } from './shared/adminuser-navbar/adminuser-navbar.component';
import { FillterComponent } from './shared/fillter/fillter.component';
import { AdminWelcomeComponent } from './pages/admin/admin-welcome/admin-welcome.component';
import { AdminTicketCategoryComponent } from './pages/admin/admin-ticket/admin-ticket-category/admin-ticket-category.component';
import { AdminNotificationsComponent } from './pages/admin/admin-notifications/admin-notifications.component';


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

    DemoMainComponent,
    Card4Component,
    Slider1Component,
    ContactUsComponent,
    Dialog1Component,
    Dialog2Component,

    Card5Component,
    DemoFormComponent,
    DemoDetailHeaderComponent,
    UserComponent,
    AdminDashboardComponent,
    AdminManagmentComponent,
    AdminManagementModalComponent,
    AdminManagementEditComponent,
    AdminUserProjectComponent,
    AdminUserProjectModalComponent,
    AdminUserProjectNewUserComponent,
    AdminUserProjectEditUserComponent,
    AdminProjectProjectComponent,
    AdminProjectComponent,
    AdminProjectModalComponent,
    AdminProjectEditComponent,
    AdminProjectNewComponent,
    AdminProjectFeaturesComponent,
    AdminWorksampleComponent,
    AdminWorksampleCreateComponent,
    AdminWorksampleEditComponent,
    AdminWorksampleCategoryComponent,
    AdminWorksampleCategoryCreateeditComponent,
    AdminCheckboxComponent,
    AdminCheckboxModalComponent,
    AdminRoleManagementComponent,
    AdminRoleManagementModalComponent,
    AdminRoleManagementAccessComponent,
    AdminPaymentComponent,
    AdminPaymentModalComponent,
    AdminTicketComponent,
    AdminTicketModalComponent,
    AdminTicketTicketComponent,
    AdminTicketPriovityComponent,
    AdminTicketPriorityModalComponent,
    AdminTicketStatusComponent,
    AdminTicketStatusModalComponent,
    AdminCounselingComponent,
    AdminCounselingModalComponent,
    AdminCounselingCounselComponent,
    AdminCountactComponent,
    AdminCountactCountactComponent,
    AdminCountactNewComponent,
    UesrDashboardComponent,
    UesrProfileComponent,
    UesrTicketComponent,
    UserTicketModalComponent,
    UserTicketTicketComponent,
    UesrProjectComponent,
    UserProjectProjectComponent,
    UserProjectNewComponent,
    Card8Component,
    Card9Component,
    Card10Component,
    Card11Component,
    Card12Component,
    Card13Component,
    AdminuserHeaderComponent,
    AdminuserNavbarComponent,
    FillterComponent,
    AdminWelcomeComponent,
    AdminTicketCategoryComponent,
    AdminNotificationsComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatDialogModule,
    MatIconModule,
    ReactiveFormsModule,
    MatTooltipModule,
    MatSidenavModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
