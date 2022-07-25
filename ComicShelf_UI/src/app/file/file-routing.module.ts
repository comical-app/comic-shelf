import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ListFilesComponent} from "./list-files/list-files.component";

const routes: Routes = [
  { path: '', component: ListFilesComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FileRoutingModule { }
