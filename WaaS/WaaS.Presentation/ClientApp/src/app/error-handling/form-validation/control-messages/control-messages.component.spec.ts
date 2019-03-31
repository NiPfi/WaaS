import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, Input } from '@angular/core';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { ValidationService } from '../validation-service/validation.service';

import { ControlMessagesComponent } from './control-messages.component';

describe('ControlMessagesComponent', () => {
  let component: ControlMessagesComponent;
  let fixture: ComponentFixture<ControlMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlMessagesComponent ],
      imports: [
        Component,
        Input,
        FormGroup,
        FormControl,
        ReactiveFormsModule
      ],
      providers: [
        ValidationService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
