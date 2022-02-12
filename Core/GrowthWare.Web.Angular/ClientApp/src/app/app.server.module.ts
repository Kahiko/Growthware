import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppComponent } from './layout/app.component';
import { AppModule } from './app.module';

@NgModule({
    imports: [AppModule, ServerModule],
    bootstrap: [AppComponent]
})
export class AppServerModule { }
