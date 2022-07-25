import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'config',
    loadChildren: () => import('./config/config.module').then(m => m.ConfigModule)
  },
  {
    path: 'file',
    loadChildren: () => import('./file/file.module').then(m => m.FileModule)
  },
  {
    path: 'library',
    loadChildren: () => import('./library/library.module').then(m => m.LibraryModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
