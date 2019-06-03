import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MockComponent, MockPipe } from 'ng-mocks';
import { AlertModule, ModalModule } from 'ngx-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';

import { ConvertNewLinePipe } from '../pipes/new-line-pipe/convert-new-line.pipe';
import { EditJobComponent } from './edit-job/edit-job.component';
import { OverviewComponent } from './overview.component';
import { ScrapeJobStatusService } from './scrape-job-status/scrape-job-status.service';
import { JobEventsComponent } from './job-events/job-events.component';

describe('OverviewComponent', () => {
  let component: OverviewComponent;
  let fixture: ComponentFixture<OverviewComponent>;
  const mockStatusService = {
    startConnection() { }
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        FontAwesomeModule,
        HttpClientTestingModule,
        NgxSpinnerModule,
        ModalModule.forRoot(),
        AlertModule
      ],
      declarations: [
        OverviewComponent,
        MockComponent(EditJobComponent),
        MockComponent(JobEventsComponent),
        MockPipe(ConvertNewLinePipe)
      ],
      providers: [
        {provide: ScrapeJobStatusService, useValue: mockStatusService}
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    spyOn(mockStatusService, 'startConnection');
    expect(component).toBeTruthy();
  });
});
