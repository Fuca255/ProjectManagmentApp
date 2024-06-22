import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SortDetailsComponent } from './sort-details.component';

describe('SortDetailsComponent', () => {
  let component: SortDetailsComponent;
  let fixture: ComponentFixture<SortDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SortDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SortDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
