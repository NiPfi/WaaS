import { async, inject, TestBed } from '@angular/core/testing';
import { DomSanitizer } from '@angular/platform-browser';

import { ConvertNewLinePipe } from './convert-new-line.pipe';

describe('ConvertNewLinePipe', () => {

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: DomSanitizer, useValue: {
            sanitize: () => 'safeString',
            bypassSecurityTrustHtml: () => 'safeString'
          }
        }
      ]
    })
    .compileComponents();
  }));

  it('create an instance', inject([DomSanitizer], (domSanitizer: DomSanitizer) => {
    const pipe = new ConvertNewLinePipe(domSanitizer);
    expect(pipe).toBeTruthy();
  }));

  it('transforms \n to <br />', inject([DomSanitizer], (domSanitizer: DomSanitizer) => {
    const pipe = new ConvertNewLinePipe(domSanitizer);
    const testString = 'abcd\nefg';
    expect(pipe.transform(testString)).toEqual('abcd<br />efg');
  }));

  it('to sanitize if enabled', inject([DomSanitizer], (domSanitizer: DomSanitizer) => {
    const pipe = new ConvertNewLinePipe(domSanitizer);
    const testString = 'abcd\nefg';
    expect(pipe.transform(testString, true)).toEqual('safeString');
  }));

});
