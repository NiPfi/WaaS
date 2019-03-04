import { Component, OnInit } from '@angular/core';
import { TestService } from './test.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  testValues: Object;

  constructor(private testService: TestService) { }

  ngOnInit() {
    this.testService.getTest().subscribe(testValues => {
      this.testValues = testValues;
    })
  }

}
