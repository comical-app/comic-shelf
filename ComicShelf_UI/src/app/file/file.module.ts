import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FileRoutingModule } from './file-routing.module';
import { ListFilesComponent } from './list-files/list-files.component';


@NgModule({
  declarations: [
    ListFilesComponent
  ],
  imports: [
    CommonModule,
    FileRoutingModule
  ]
})
export class FileModule { }
