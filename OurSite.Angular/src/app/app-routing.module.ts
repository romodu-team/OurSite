import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsComponent } from './pages/about-us/about-us.component';
import { AdminCheckboxComponent } from './pages/admin/admin-checkbox/admin-checkbox.component';
import { AdminCounselingComponent } from './pages/admin/admin-counseling/admin-counseling.component';
import { AdminCountactComponent } from './pages/admin/admin-countact/admin-countact.component';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard/admin-dashboard.component';
import { AdminManagmentComponent } from './pages/admin/admin-managment/admin-managment.component';
import { AdminNotificationsComponent } from './pages/admin/admin-notifications/admin-notifications.component';
import { AdminPaymentComponent } from './pages/admin/admin-payment/admin-payment.component';
import { AdminProjectComponent } from './pages/admin/admin-project/admin-project.component';
import { AdminRoleManagementComponent } from './pages/admin/admin-role-management/admin-role-management.component';
import { AdminTicketCategoryComponent } from './pages/admin/admin-ticket/admin-ticket-category/admin-ticket-category.component';
import { AdminTicketPriovityComponent } from './pages/admin/admin-ticket/admin-ticket-priovity/admin-ticket-priovity.component';
import { AdminTicketStatusComponent } from './pages/admin/admin-ticket/admin-ticket-status/admin-ticket-status.component';
import { AdminTicketComponent } from './pages/admin/admin-ticket/admin-ticket.component';
import { AdminUserProjectComponent } from './pages/admin/admin-user-project/admin-user-project.component';
import { AdminWelcomeComponent } from './pages/admin/admin-welcome/admin-welcome.component';
import { AdminWorksampleCategoryComponent } from './pages/admin/admin-worksample/admin-worksample-category/admin-worksample-category.component';
import { AdminWorksampleComponent } from './pages/admin/admin-worksample/admin-worksample.component';
import { ContactUsComponent } from './pages/contact-us/contact-us.component';
import { DemoMainComponent } from './pages/demo/demo-main/demo-main.component';
import { FeatureComponent } from './pages/feature/feature.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { UserComponent } from './pages/user/user.component';
import { AuthGuard } from './shared/services/auth.guard';
import { DemoFormComponent } from './shared/demo-form/demo-form.component';
import { AdminManagementEditComponent } from './pages/admin/admin-managment/admin-management-edit/admin-management-edit.component';
import { AdminUserProjectEditUserComponent } from './pages/admin/admin-user-project/admin-user-project-edit-user/admin-user-project-edit-user.component';
import { AdminProjectEditComponent } from './pages/admin/admin-project/admin-project-edit/admin-project-edit.component';
import { AdminProjectFeaturesComponent } from './pages/admin/admin-project/admin-project-features/admin-project-features.component';
import { AdminWorksampleEditComponent } from './pages/admin/admin-worksample/admin-worksample-edit/admin-worksample-edit.component';
import { AdminWorksampleCategoryCreateeditComponent } from './pages/admin/admin-worksample/admin-worksample-category/admin-worksample-category-createedit/admin-worksample-category-createedit.component';

const routes: Routes = [
  { path:'', component:HomeComponent },
  { path:'demo', component:DemoMainComponent },
  { path:'contact', component:ContactUsComponent },
  { path:'about', component:AboutUsComponent },
  { path:'feature', component: FeatureComponent },
  { path:'login', component: LoginComponent },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard] },
  { path: 'admin', component: AdminWelcomeComponent, children: [
    { path: 'more/checkbox', component: AdminCheckboxComponent },
    { path: 'more/notifications', component: AdminNotificationsComponent },

    { path: 'counseling', component: AdminCounselingComponent },
    { path: 'contact', component: AdminCountactComponent },

    { path: 'admin/management', component: AdminManagmentComponent },
    { path: 'admin/management/edit', component: AdminManagementEditComponent },
    { path: 'admin/project', component: AdminProjectComponent },
    { path: 'admin/project/edit', component: AdminProjectEditComponent },
    { path: 'admin/project/features', component: AdminProjectFeaturesComponent },
    { path: 'admin/role-management', component: AdminRoleManagementComponent },
    { path: 'admin/worksample/worksample', component: AdminWorksampleComponent },
    { path: 'admin/worksample/worksample/edit', component: AdminWorksampleEditComponent },
    { path: 'admin/worksample/category', component: AdminWorksampleCategoryComponent },
    { path: 'admin/worksample/category/edit', component: AdminWorksampleCategoryCreateeditComponent },
    { path: 'admin/user-project', component: AdminUserProjectComponent },
    { path: 'admin/user-project/edit', component: AdminUserProjectEditUserComponent },

    { path: 'ticket/ticket', component: AdminTicketComponent },
    { path: 'ticket/category', component: AdminTicketCategoryComponent },
    { path: 'ticket/status', component: AdminTicketStatusComponent },
    { path: 'ticket/priority', component: AdminTicketPriovityComponent },

    { path: 'payment', component: AdminPaymentComponent },
    { path: 'dashboard', component: AdminDashboardComponent },
  ] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
