import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPipe } from 'ng-mocks';
import { AlertModule } from 'ngx-bootstrap';
import { CookieModule } from 'ngx-cookie';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { VerifyMailChangeComponent } from './verify-mail-change.component';

describe('VerifyMailChangeComponent', () => {
  let component: VerifyMailChangeComponent;
  let fixture: ComponentFixture<VerifyMailChangeComponent>;

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
        VerifyMailChangeComponent,
        MockPipe(ConvertNewLinePipe)
      ],
      providers: [
        {provide: ActivatedRouteSnapshot, useValue: activatedRouteSnapshotStub}
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyMailChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
