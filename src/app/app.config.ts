//angular configuraion file
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import {
provideHttpClient,
withInterceptors
}
from '@angular/common/http';
import {
authInterceptor
}
from './interceptors/auth.interceptor';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = { //configuring angular
  providers: [  //services provided
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes), //use routes
    provideHttpClient(
  withInterceptors([
    authInterceptor
  ])
)//enable api connect the backend
  ]
};