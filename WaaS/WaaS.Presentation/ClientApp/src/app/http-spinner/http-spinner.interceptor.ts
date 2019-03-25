import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs/internal/Observable';
import { tap } from 'rxjs/operators';

@Injectable()
export class HttpSpinnerInterceptor implements HttpInterceptor {

    constructor(
        private readonly spinner: NgxSpinnerService
    ) { }

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {

        this.spinner.show();

        return next.handle(req).pipe(tap(
            (event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    this.spinner.hide();
                }
            },
            error => {
                this.spinner.hide();
            }
        ));

    }

}
