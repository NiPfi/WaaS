import { Pipe, PipeTransform, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'convertNewLine'
})
export class ConvertNewLinePipe implements PipeTransform {

  constructor(private readonly sanitizer: DomSanitizer) {}

  transform(value: string, sanitize = false): string {
    if (typeof value !== 'string') {
      return value;
    }
    let result: any;
    const textParsed = value.replace(/(?:\r\n|\r|\n)/g, '<br />');

    if (sanitize) {
      result = this.sanitizer.sanitize(SecurityContext.HTML, textParsed);
    } else {
      result = textParsed;
    }

    return result;
  }

}
