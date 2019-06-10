import { ScrapeJobStatusCode } from './scrape-job-status-code';

export interface ScrapeJobStatus {
  scrapeJobId: number;
  statusCode: ScrapeJobStatusCode;
}
