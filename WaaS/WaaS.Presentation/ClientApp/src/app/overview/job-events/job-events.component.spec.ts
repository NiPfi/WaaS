import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalModule, AlertModule } from 'ngx-bootstrap';
import { ConvertNewLinePipe } from '../../pipes/new-line-pipe/convert-new-line.pipe';
import { MockPipe } from 'ng-mocks';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { JobEventsComponent } from './job-events.component';

describe('JobEventsComponent', () => {
  let component: JobEventsComponent;
  let fixture: ComponentFixture<JobEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ModalModule.forRoot(),
        AlertModule,
        HttpClientTestingModule
      ],
      declarations: [ 
        JobEventsComponent,
        MockPipe(ConvertNewLinePipe)
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
