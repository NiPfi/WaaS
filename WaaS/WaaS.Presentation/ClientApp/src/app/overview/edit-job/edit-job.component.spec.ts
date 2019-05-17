import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ReactiveFormsModule } from '@angular/forms';
import { MockComponent } from 'ng-mocks';
import { AlertModule, ModalModule } from 'ngx-bootstrap';
import { ConvertNewLinePipe } from 'src/app/pipes/new-line-pipe/convert-new-line.pipe';

import { ControlMessagesComponent } from 'src/app/error-handling/form-validation/control-messages/control-messages.component';
import { EditJobComponent } from './edit-job.component';

describe('EditJobComponent', () => {
  let component: EditJobComponent;
  let fixture: ComponentFixture<EditJobComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FontAwesomeModule,
        HttpClientTestingModule,
        ModalModule.forRoot(),
        AlertModule
      ],
      declarations: [ 
        EditJobComponent,
        MockComponent(ControlMessagesComponent),
        ConvertNewLinePipe
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
