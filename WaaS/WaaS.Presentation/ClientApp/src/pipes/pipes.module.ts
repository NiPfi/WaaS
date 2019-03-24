import { NgModule } from '@angular/core';

import { ConvertNewLinePipe } from './new-line-pipe/convert-new-line.pipe';

@NgModule({
  declarations: [
    ConvertNewLinePipe
  ],
  exports: [
    ConvertNewLinePipe
  ],
  imports: [
  ]
})
export class PipesModule { }
