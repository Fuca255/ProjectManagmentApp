import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProjectOverlayComponent } from './edit-project-overlay.component';

describe('EditProjectOverlayComponent', () => {
  let component: EditProjectOverlayComponent;
  let fixture: ComponentFixture<EditProjectOverlayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditProjectOverlayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditProjectOverlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
