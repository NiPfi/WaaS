import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPipe } from 'ng-mocks';
import { AlertModule } from 'ngx-bootstrap';
import { CookieModule } from 'ngx-cookie';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { VerifyRegistrationComponent } from './verify-registration.component';

describe('VerifyRegistrationComponent', () => {
  let component: VerifyRegistrationComponent;
  let fixture: ComponentFixture<VerifyRegistrationComponent>;

  beforeEach(async(() => {
    const activatedRouteSnapshotStub: any = {
      paramMap: {
        get() {
          return null;
        }
      }
    };

    TestBed.configureTestingModule({
      imports: [
        AlertModule,
        RouterTestingModule,
        HttpClientTestingModule,
        CookieModule.forRoot()
      ],
      declarations: [
        VerifyRegistrationComponent,
        MockPipe(ConvertNewLinePipe)
      ],
      providers: [
        {provide: ActivatedRouteSnapshot, useValue: activatedRouteSnapshotStub}
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
