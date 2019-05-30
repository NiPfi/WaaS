import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobEventsComponent } from './job-events.component';

describe('JobEventsComponent', () => {
  let component: JobEventsComponent;
  let fixture: ComponentFixture<JobEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobEventsComponent ]
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
