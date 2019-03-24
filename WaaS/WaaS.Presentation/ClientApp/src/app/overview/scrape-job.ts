import { Url } from 'url';

export class ScrapeJob {
  name: string;
  url: Url;
  pattern: RegExp;
  enabled: boolean;
}
