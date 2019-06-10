import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { AuthService } from 'src/app/authentication/auth.service';
import { environment } from 'src/environments/environment';

import { ScrapeJobStatus } from './scrape-job-status';

@Injectable({
  providedIn: 'root'
})
export class ScrapeJobStatusService {

  private statusSource: BehaviorSubject<ScrapeJobStatus[]>;
  status: Observable<ScrapeJobStatus[]>;

  private readonly connection: signalR.HubConnection;

  constructor(
    private authService: AuthService
  ) {
    this.statusSource = new BehaviorSubject<ScrapeJobStatus[]>([]);
    this.status = this.statusSource.asObservable();
    this.connection = new signalR.HubConnectionBuilder()
    .withUrl(
      `${environment.signalrUrl}/scrapejob/status`,
      { accessTokenFactory: () => this.authService.getUserToken() }
    )
    .configureLogging(signalR.LogLevel.Trace)
    .build();

    this.connection.serverTimeoutInMilliseconds = 1000 * 60;
  }

  public startConnection(): void {
    this.connection.start();
    this.connection.on('statusUpdate', this.handleStatusUpdate);
  }

  public closeConnection(): void {
    this.connection.stop();
  }

  private handleStatusUpdate(status: ScrapeJobStatus[]) {
    this.statusSource.next(status);
  }
}
