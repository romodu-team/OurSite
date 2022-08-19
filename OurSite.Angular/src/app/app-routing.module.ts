import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsComponent } from './pages/about-us/about-us.component';
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
  { path: 'user', component: UserComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
