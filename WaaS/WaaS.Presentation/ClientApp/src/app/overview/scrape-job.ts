import { UrlObject } from 'url';

export class ScrapeJob {
  id: number;
  name: string;
  url: UrlObject;
  pattern: RegExp;
  enabled: boolean;
}
