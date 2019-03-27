import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CookieModule } from 'ngx-cookie';

import { EditProfileService } from './edit-profile.service';

describe('EditProfileService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule,
      RouterTestingModule,
      CookieModule.forRoot()
    ]
  }));

  it('should be created', () => {
    const service: EditProfileService = TestBed.get(EditProfileService);
    expect(service).toBeTruthy();
  });
});
