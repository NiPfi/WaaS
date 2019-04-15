import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { catchError } from 'rxjs/internal/operators/catchError';
import { map } from 'rxjs/internal/operators/map';
import { HttpErrorHandlerService } from 'src/app/error-handling/http-error-handler.service';
import { environment } from 'src/environments/environment';

import { ScrapeJob } from '../scrape-job';

@Injectable({
  providedIn: 'root'
})
export class OverviewService{

  constructor(
    private readonly http: HttpClient,
    private readonly handler: HttpErrorHandlerService
  ) { }

  getScrapeJobs(): Observable<ScrapeJob[]> {
    return this.http.get(`${environment.apiUrl}/scrapejob`)
      .pipe(catchError(this.handler.handleError))
      .pipe(map((response: ScrapeJob[]) => response));
  }

  addScrapeJob(job: ScrapeJob): Observable<{}> {
    return this.http.post(`${environment.apiUrl}/scrapejob`, job)
      .pipe(catchError(this.handler.handleError));
  }
}
