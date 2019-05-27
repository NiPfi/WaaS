import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment } from 'src/environments/environment';

import { ScrapeJobStatus } from './scrape-job-status';

@Injectable({
  providedIn: 'root'
})
export class ScrapeJobStatusService {

  public status: ScrapeJobStatus[];

  private readonly connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
    .withUrl(`${environment.signalrUrl}/scrapejob/status`)
    .configureLogging(signalR.LogLevel.Trace)
    .build();

    this.connection.serverTimeoutInMilliseconds = 1000 * 60;
  }

  public startConnection(): void {
    console.log(this.connection.state);
    console.log('startConnection');
    this.connection.start();
    this.connection.on("statusUpdate", this.handleStatusUpdate);
  }

  public closeConnection(): void {
    this.connection.stop();
  }

  private handleStatusUpdate() {
    console.log('Received status information');
  }
}
