import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsComponent } from './pages/about-us/about-us.component';
import { AdminCheckboxComponent } from './pages/admin/admin-checkbox/admin-checkbox.component';
import { AdminCounselingComponent } from './pages/admin/admin-counseling/admin-counseling.component';
import { AdminCountactComponent } from './pages/admin/admin-countact/admin-countact.component';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard/admin-dashboard.component';
import { AdminManagmentComponent } from './pages/admin/admin-managment/admin-managment.component';
import { AdminPaymentComponent } from './pages/admin/admin-payment/admin-payment.component';
import { AdminProjectComponent } from './pages/admin/admin-project/admin-project.component';
import { AdminRoleManagementComponent } from './pages/admin/admin-role-management/admin-role-management.component';
import { AdminTicketComponent } from './pages/admin/admin-ticket/admin-ticket.component';
import { AdminUserProjectComponent } from './pages/admin/admin-user-project/admin-user-project.component';
import { AdminWelcomeComponent } from './pages/admin/admin-welcome/admin-welcome.component';
import { AdminWorksampleComponent } from './pages/admin/admin-worksample/admin-worksample.component';
import { ContactUsComponent } from './pages/contact-us/contact-us.component';
import { DemoMainComponent } from './pages/demo/demo-main/demo-main.component';
import { FeatureComponent } from './pages/feature/feature.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { UserComponent } from './pages/user/user.component';
import { AuthGuard } from './services/auth.guard';
import { DemoFormComponent } from './shared/demo-form/demo-form.component';

const routes: Routes = [
  { path:'', component:HomeComponent },
  { path:'demo', component:DemoMainComponent },
  { path:'contact', component:ContactUsComponent },
  { path:'about', component:AboutUsComponent },
  { path:'feature', component: FeatureComponent },
  { path:'login', component: LoginComponent },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard] },
  { path: 'admin', component: AdminWelcomeComponent, children: [
    { path: 'checkbox', component: AdminCheckboxComponent },
    { path: 'counseling', component: AdminCounselingComponent },
    { path: 'contact', component: AdminCountactComponent },
    { path: 'management', component: AdminManagmentComponent },
    { path: 'payment', component: AdminPaymentComponent },
    { path: 'project', component: AdminProjectComponent },
    { path: 'role-management', component: AdminRoleManagementComponent },
    { path: 'ticket', component: AdminTicketComponent },
    { path: 'user-project', component: AdminUserProjectComponent },
    { path: 'worksample', component: AdminWorksampleComponent },
    { path: 'dashboard', component: AdminDashboardComponent },
  ] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
