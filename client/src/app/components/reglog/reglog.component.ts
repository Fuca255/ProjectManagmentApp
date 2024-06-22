import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-reglog',
  templateUrl: './reglog.component.html',
  styleUrls: ['./reglog.component.css']
})
export class ReglogComponent implements OnInit{
  componentName: string = 'login'; // Postavljamo 'login' kao početno stanje

  constructor() { }

  ngOnInit(): void {
  }

  onKey(componentName: string): void {
    this.componentName = componentName;
  }
}
