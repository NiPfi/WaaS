import { ScrapeJobStatusCode } from './scrape-job-status/scrape-job-status-code';

export class ScrapeJob {
  id: number;
  name: string;
  url: string;
  pattern: string;
  enabled: boolean;
  alternativeEmail: string;
  status: ScrapeJobStatusCode;
}
