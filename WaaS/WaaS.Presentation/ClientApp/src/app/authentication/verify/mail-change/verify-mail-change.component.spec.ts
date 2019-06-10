import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPipe } from 'ng-mocks';
import { AlertModule } from 'ngx-bootstrap';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { VerifyMailChangeComponent } from './verify-mail-change.component';

describe('VerifyMailChangeComponent', () => {
  let component: VerifyMailChangeComponent;
  let fixture: ComponentFixture<VerifyMailChangeComponent>;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        AlertModule,
        HttpClientTestingModule,
        RouterTestingModule
      ],
      declarations: [
        VerifyMailChangeComponent,
        MockPipe(ConvertNewLinePipe)
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
