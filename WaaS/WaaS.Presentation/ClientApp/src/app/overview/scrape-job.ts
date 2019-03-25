import { Url } from 'url';

export class ScrapeJob {
  id: number;
  name: string;
  url: Url;
  pattern: RegExp;
  enabled: boolean;
}
