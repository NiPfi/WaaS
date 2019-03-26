import { isPlatformBrowser } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { throwError } from 'rxjs';

import { ApiError } from '../authentication/api-error';


@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerService {
  private readonly isBrowser: boolean;

  constructor(
    @Inject(PLATFORM_ID) platformId: object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public handleError(error: HttpErrorResponse) {
    if (this.isBrowser) {
      if (error.error instanceof ErrorEvent) {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('An error occurred:', error.error.message);
        return throwError(`There was an error sending your request: ${error.error.message}`);
      } else {
        const apiError = error.error as ApiError;
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong,
        console.error(
          `Backend returned code ${apiError.statusCode}: ${apiError.statusDescription}, ` +
          `Message: ${apiError.message}`);

        if (error.message) {
          return throwError(apiError.message);
        } else {
          return throwError(`An error has occurred. Details have been logged to the console.`);
        }
      }
      // return an observable with a user-facing error message
    } else {
      console.error('An error occurred during prerendering');
      return throwError(error);
    }
  }
}
