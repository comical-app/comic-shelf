import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoggedComponent} from "../template-layout/logged/logged.component";
import {ListUserComponent} from "./users/list-user/list-user.component";
import {AddUserComponent} from "./users/add-user/add-user.component";

const routes: Routes = [
  {
    path: 'users', component: LoggedComponent, children: [
      {path: 'add', component: AddUserComponent},
      {path: '', component: ListUserComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigRoutingModule {
}
