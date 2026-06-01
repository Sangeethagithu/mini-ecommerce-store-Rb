//angular configuraion file
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = { //configuring angular
  providers: [  //services provided
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), //use routes
    provideHttpClient()//enable api connect the backend
  ]
};