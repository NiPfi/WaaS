import { HttpClientTestingModule } from '@angular/common/http/testing';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ReactiveFormsModule } from '@angular/forms';
import { AlertModule, ModalModule } from 'ngx-bootstrap';

import { AddJobComponent } from './add-job.component';

describe('AddJobComponent', () => {
  let component: AddJobComponent;
  let fixture: ComponentFixture<AddJobComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FontAwesomeModule,
        HttpClientTestingModule,
        ModalModule.forRoot(),
        AlertModule
      ],
      declarations: [ AddJobComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
