import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWeeklyAnalizeComponent } from './project-weekly-analize.component';

describe('ProjectWeeklyAnalizeComponent', () => {
  let component: ProjectWeeklyAnalizeComponent;
  let fixture: ComponentFixture<ProjectWeeklyAnalizeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectWeeklyAnalizeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProjectWeeklyAnalizeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
