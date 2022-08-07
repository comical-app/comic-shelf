import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {FileModule} from "./file/file.module";
import {ConfigModule} from "./config/config.module";
import {LibraryModule} from "./library/library.module";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { NotLoggedComponent } from './template-layout/not-logged/not-logged.component';
import { LoggedComponent } from './template-layout/logged/logged.component';
import { MenuNavbarComponent } from './template-layout/menu-navbar/menu-navbar.component';
import {TemplateLayoutModule} from "./template-layout/template-layout.module";

@NgModule({
  declarations: [
    AppComponent,
    NotLoggedComponent,
    LoggedComponent,
    MenuNavbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FileModule,
    ConfigModule,
    LibraryModule,
    TemplateLayoutModule,
    NgbModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
