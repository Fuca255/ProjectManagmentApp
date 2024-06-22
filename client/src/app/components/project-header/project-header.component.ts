import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-project-header',
  templateUrl: './project-header.component.html',
  styleUrl: './project-header.component.css'
})
export class ProjectHeaderComponent implements OnInit{
  currentView: string = 'kanban';
  searchText: string = "";
  @Output() viewChange: EventEmitter<string> = new EventEmitter<string>();
  @Output() getSearchText: EventEmitter<string> = new EventEmitter<string>();


  ngOnInit(): void {
    const savedView = localStorage.getItem('currentView');
    if (savedView) {
      this.currentView = savedView;
    }
  }

  getText()
  {
    this.getSearchText.emit(this.searchText);
  }


  changeView(view: string) {
    this.viewChange.emit(view);
    this.currentView = view;
    localStorage.setItem('currentView', view);
  }
}
