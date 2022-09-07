import { Location } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EssService {

  constructor(private loc: Location) { }

  back(){
    this.loc.back()
  }
}
