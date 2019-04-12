import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPipe } from 'ng-mocks';
import { AlertModule } from 'ngx-bootstrap';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { VerifyComponent } from './verify.component';

describe('VerifyComponent', () => {
  let component: VerifyComponent;
  let fixture: ComponentFixture<VerifyComponent>;

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
        HttpClientTestingModule
      ],
      declarations: [
        VerifyComponent,
        MockPipe(ConvertNewLinePipe)
      ],
      providers: [
        {provide: ActivatedRouteSnapshot, useValue: activatedRouteSnapshotStub}
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
