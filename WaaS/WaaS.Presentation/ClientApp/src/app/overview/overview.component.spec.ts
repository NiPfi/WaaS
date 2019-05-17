import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MockComponent } from 'ng-mocks';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule, AlertModule } from 'ngx-bootstrap';
import { MockPipe } from 'ng-mocks';

import { ConvertNewLinePipe } from '../pipes/new-line-pipe/convert-new-line.pipe';
import { EditJobComponent } from './edit-job/edit-job.component';
import { OverviewComponent } from './overview.component';

describe('OverviewComponent', () => {
  let component: OverviewComponent;
  let fixture: ComponentFixture<OverviewComponent>;

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
        MockPipe(ConvertNewLinePipe)
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
    expect(component).toBeTruthy();
  });
});
