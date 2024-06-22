import { AfterViewInit, EventEmitter,Component, ElementRef, HostBinding, Input, OnInit, Output, TemplateRef, ViewChild, inject, OnChanges, SimpleChanges } from '@angular/core';
import {srLatn} from 'date-fns/locale'
import { JwtDecoderService } from '../../services/jwt-decoder.service';
import { NgToastService } from 'ng-angular-popup';
import {GantogramService} from '../../services/gantogram.service'
import { GanttMapper } from '../../models/gantogram/gantt_mapper';
import { DomSanitizer } from '@angular/platform-browser';
import {
  GANTT_GLOBAL_CONFIG,
  GanttBarClickEvent,
  GanttBaselineItem,
  GanttDragEvent,
  GanttGroup,
  GanttItem,
  GanttLineClickEvent,
  GanttLinkDragEvent,
  GanttLinkLineType,
  GanttPrintService,
  GanttSelectedEvent,
  GanttTableDragDroppedEvent,
  GanttTableDragEndedEvent,
  GanttTableDragEnterPredicateContext,
  GanttTableDragStartedEvent,
  GanttToolbarOptions,
  GanttView,
  GanttViewType,
  NgxGanttComponent 
} from '@worktile/gantt';
import { Observable, Subject, delay, filter, finalize, of } from 'rxjs';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { Task } from '../../models/task/task';
import { ActivatedRoute } from '@angular/router';

import html2Canvas from 'html2canvas'
import { UpdateGant } from '../../models/gantogram/update_gant_task';
import { DatePipe } from '@angular/common';

import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MatDialog } from '@angular/material/dialog';
import { TaskDetailsComponent } from '../../pages/task-details/task-details.component';
import { AssignmentService } from '../../services/assignment.service';
import { Role } from '../../models/role';
import { RoleService } from '../../services/role.service';
import { TaskFilter } from '../../models/task/task-filter';
import { Notify } from '../../models/notifications/notify';
// import { random, randomGroupsAndItems, randomItems } from '../../helper';





@Component({
  selector: 'app-gantogram',
  templateUrl: './gantogram.component.html',
  styleUrl: './gantogram.component.css',
  template: ``,
  providers: [GanttPrintService,  DatePipe,
    {
      provide: GANTT_GLOBAL_CONFIG,
      useValue: {
        linkOptions: {
          // dependencyTypes: 1, // fs | ff | ss | sf
          showArrow: true, // 连接线是否显示箭头
          lineType: GanttLinkLineType.curve, // 连接线类型（曲线或直线）

        },
        styleOptions: {
          headerHeight: 80,
          lineHeight: 70,
          barHeight: 60
      }
      }
    }
  ],
  
})
export class GantogramComponent implements OnInit, AfterViewInit,OnChanges{
  
  @Input() searchText!: string;
  private  modalService = inject(NgbModal);
  private decoder = inject(JwtDecoderService);
  private userId : number = 0;
  userProjectRole! : Role;//dodati role servis i uzeti project role
  private roleService = inject(RoleService);
  private projectId : number = 0;
  private route = inject(ActivatedRoute);
  private ganttService = inject(GantogramService)
  private assignmentService = inject(AssignmentService)
  private itemsOldState: GanttItem[] = [];
  private notify : Notify
  
  isDragable:boolean = false;
  isLinkable:boolean = false;


  // it: GanttItem[] = [];

  groups: GanttGroup[] = [];

  expanded = true;
  
  task!: Task;
  taskFilter : TaskFilter = {}
  private dialog = inject(MatDialog);
  @Output() newItemEvent = new EventEmitter<{previous_state : number, task: Task}>();

  views = [
      
      {
          name: 'Day',
          value: GanttViewType.day
      },
      {
          name: 'Month',
          value: GanttViewType.month
      },
      {
          name: 'Quarter',
          value: GanttViewType.quarter
      }
    ];
    
    
    viewType: GanttViewType = GanttViewType.month;
    
    selectedViewType: GanttViewType = GanttViewType.month;
    
    isBaselineChecked = true;
    
    isShowToolbarChecked = false;
    
    loading = false;
    
    items: GanttItem[] = [];
    
    selectedTask: any
    selectedAction: boolean = false;
    
  toolbarOptions: GanttToolbarOptions = {
      viewTypes: [GanttViewType.day, GanttViewType.month, GanttViewType.year]
      
  };

  baselineItems: GanttBaselineItem[] = [];

  options = {
      viewType: GanttViewType.day
  };
  
  
  viewOptions = {
      dateFormat: {
        week: "w", // 第w周
        month: "M", // M月
        quarter: "QQQ", // QQQ
        year: "yyyy", // yyyy年
        yearMonth: "yyyy-MM", // yyyy年MM月
        yearQuarter: "yyyy-QQQ" // yyyy年QQQ
    },
    styleOptions: {
        headerHeight: 150,
        lineHeight: 120,
        barHeight: 150
    },
    dateOptions: {
        locale: srLatn, // 时区  import { fr } from 'date-fns/locale';
        weekStartsOn: 0 | 1 | 2 | 3 | 4 | 5 | 6 // 设置 week 起始值，默认为 1
      }
  };

  

  @HostBinding('class.gantt-example-component') class = true;
  @ViewChild('gantt') ganttComponent!: NgxGanttComponent;
  @ViewChild('screenshotElement') screenshotElement!: NgxGanttComponent;
  @ViewChild('content') contentRef!: TemplateRef<any>;


  dropEnterPredicate = (event: GanttTableDragEnterPredicateContext) => {
    return true;
  };
  private spinner = inject(NgxSpinnerService);

  constructor(private printService: GanttPrintService,private toast : NgToastService,private datePipe: DatePipe,private sanitizer: DomSanitizer) {
    this.projectId = Number(this.route.snapshot.paramMap.get('projectId'));
    this.notify = new Notify(toast);
    // this.spinner.show();
    
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.taskFilter.searchTitle = this.searchText;
    this.getGanttItemsByProjectId();
  }

 

  ngOnInit(): void {
      // init items children

      // const { groups, items } = randomGroupsAndItems(10);
      // this.groups_test = groups;
      // this.items_test = items;

      // console.log(groups);
      // console.log(items);
      


      this.isDragable = true;
      this.isLinkable = true;
      let token = this.decoder.getToken();
      if(token!=null)
      {
        let decode = this.decoder.decodeToken(token);
        this.userId = decode.user_id;
        this.roleService.getUserProjectRole(this.userId,this.projectId).subscribe({
          next: (role:Role)=>{
            this.userProjectRole = role;
            
            if(role.name=="Project Manager"){
              this.isDragable = true;
              this.isLinkable = true;              
            }
            this.getGanttItemsByProjectId();
            // this.spinner.hide();
          }
        });
      }

      
      // this.spinner.show();
      // this.items.forEach((item, index) => {
      //     if (index % 5 === 0) {
      //         item.children = randomItems(random(1, 5), item);
      //     }
      // });
      // this.spinner.hide();
    
  }
  // ---------------------------------------------------------------------
  expandAllGroups() {
    if (this.expanded) {
        this.expanded = false;
        this.ganttComponent.collapseAll();
    } else {
        this.expanded = true;
        this.ganttComponent.expandAll();
    }
  }

//   childrenResolve = (item: GanttItem) => {
//     const children = randomItems(random(1, 5), item);
//     return of(children).pipe(delay(1000));
// };

  // ------------------------------------------------------------------------


  public getUpdateEmitter(task:Task)
  {
    if(this.task.id===task.id)
    {
      this.task = task;
    }
  }

  openShowTaskOverlay(task : Task): void {
    // console.log(task);
    const dialogRef = this.dialog.open(TaskDetailsComponent, {
      data:[task,0,this.userProjectRole]
    });

    dialogRef.afterClosed().subscribe(result => {
      this.newItemEvent.emit({previous_state : task.state.id,task:result});
      // console.log(this.task);
      // console.log(result);
      this.task = result;
    });
  }

  getTaskByID(id:string){
      this.assignmentService.getAssignmentById(parseInt(id)).subscribe({
        next : (task: Task)=> 
          {
            if (task){
              task.start = new Date(task.start);
              task.end = new Date(task.end);
              this.openShowTaskOverlay(task);

            } else {
                // console.log("No tasks found or tasks[0] is undefined.");
            }
          },
        // error:(error: any)=> console.log(error)
      });
  }
  

  openVerticallyCentered(content: TemplateRef<any>): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      let modalRef: NgbModalRef;
      this.selectedAction = false; // Resetujemo vrednost selectedAction
      modalRef = this.modalService.open(content, { centered: true });
  
      modalRef.result.then((result) => {
        // console.log('Modal zatvoren:', result);
        // Ovde možemo obraditi povratnu vrednost i izvršiti potrebne akcije
        this.selectedAction = result;
        resolve(this.selectedAction);
      }, (reason) => {
        // console.log('Modal zatvoren bez akcije:', reason);
        reject(reason);
      });
    });
  }

  copyItems(items: GanttItem[]): GanttItem[] {
    // Koristimo map metodu za pravljenje kopije svakog elementa niza
    return items.map(item => ({ ...item }));
  }

  getGanttItemsByProjectId(){
    this.loading = true;
    this.assignmentService.getAllProjectAssignments(this.projectId,this.taskFilter).subscribe({
      next : (tasks: Task[])=> 
        {
          if (tasks && tasks.length > 0){
            var res = GanttMapper.mapTasksToGantItems(tasks,this.groups)
            this.items = res
            this.itemsOldState = this.copyItems(this.items)
            this.loading = false;
          } else {
              // console.log("No tasks found or tasks[0] is undefined.");
          }
        },
      // error:(error: any)=> console.log(error)
    });
  }
  private updateSuccessSubject = new Subject<boolean>();

  public emitUpdateSuccess(success: boolean): void {
    this.updateSuccessSubject.next(success);
  }

  public getUpdateSuccessObservable(): Observable<boolean> {
    return this.updateSuccessSubject.asObservable();
  }

  updateGantItemById(assign_id:number, update :UpdateGant, ){
    this.loading = true;
    // console.log(update);
    
    this.ganttService.updateTaskById(update,assign_id).subscribe({
      next : (task: Task)=> 
        {
          this.loading = false;
          const success = task !== null;
          this.emitUpdateSuccess(success)
          this.itemsOldState = this.copyItems(this.items)
        },
      error:(error: any)=> this.notify.showError("Update error",error)
    });

  }


  ngAfterViewInit() {
      setTimeout(() => this.ganttComponent.scrollToDate(new Date().getTime()), 200);
  }

  barClick(event: GanttBarClickEvent) {
    this.getTaskByID(event.item.id)
    //  ovde treba otvoriti modal za task details 
    // this.showSuccess("BarClick", `Ovo je poruka za success obavestenje kada se klikne na bar gantograma [id = ${event.item.id}] [Task Name = ${event.item.title}`);
  }
//   kada kliknemo na vezxu tj liniju izmedju dva taska (bara)
  lineClick(event: GanttLineClickEvent) {

    this.selectedTask = event;

    this.openVerticallyCentered(this.contentRef).then((result) => {
      if (result === true) {
        let update: UpdateGant = {
          removeDependentOn: [parseInt(event.source.id)], 
        };
        this.updateGantItemById(parseInt(event.target.id),update);
        this.getUpdateSuccessObservable().subscribe((success: boolean) => {
          if (success) {
            this.notify.showSuccess("Uspesno obrisana zavisnost",`Uspešno obrisana zavisnost. [${event.source.title}]->[${event.target?.title}]`)
            this.getGanttItemsByProjectId();
            
          } else {
            this.notify.showError("Greška",`Greška pri brisanju zavisnosti taska  ${event.source.title}`);
            this.getGanttItemsByProjectId();
          }
        });
      }
    }).catch((error) => {
      console.error("Greška pri otvaranju moda:", error);
    });
  }

//    kada se uhvati bar da se pomera
  dragMoved(event: GanttDragEvent) {
    // ovde treba pozvati update start end date 
    this.notify.showInfo("Izmena trajanja taska",`Da biste promenili vreme na gantogramu, pratite ove korake: \n\n

    Pomeranje granica: Kliknite na levu ili desnu granicu vremenskog okvira koji želite da promenite. Držite taster miša pritisnutim i pomerajte miša levo ili desno kako biste pomerili granicu na odgovarajuće mesto. \n\n
    
    Pomeranje trake: Kliknite na samu traku vremenskog okvira i prevucite je levo ili desno kako biste je pomerali unapred ili unazad kroz vreme.`);
  }
//   Kada se pusti bar nakon pomeranja
  dragEnded(event: GanttDragEvent) {
    let update: UpdateGant = {
      startTs: event.item.start, 
      endTs: event.item.end,
    };
    this.updateGantItemById(parseInt(event.item.id),update);
    

    this.getUpdateSuccessObservable().subscribe((success: boolean) => {
      if(success && update.startTs !== undefined && update.endTs !== undefined){
        let start = new Date(update.startTs*1000);
        let end  = new Date(update.endTs*1000 );
        const formattedStart = this.datePipe.transform(start, 'medium');
        const formattedEnd = this.datePipe.transform(end, 'medium');
        this.notify.showSuccess("Uspesno izmenjeno vreme","Promenjeno vreme trajanja taska. \n Pocetak-["+ formattedStart +"]\nKraj-["+ formattedEnd + "]" )
      }else{
        this.notify.showError("Greška","Greška pri izmeni vremena trajanja taska - "+event.item.title)
        this.getGanttItemsByProjectId()
      }
    });    
  }

  selectedChange(event: GanttSelectedEvent) {
      if (event.current !== undefined) {
          const startDate = event.current.start;
          if (startDate !== undefined) {
              this.ganttComponent.scrollToDate(startDate);
          }
      }
      this.notify.showInfo("SelectedChange",`Selektovao sam task ${event.current?.title}`)
  }
  

  linkDragEnded(event: GanttLinkDragEvent) {
    if(event.target !== undefined ){
      // console.log(this.itemsOldState.find(item => item.id === event.source.id));
      if(GanttMapper.checkIfNumberExists(event.source.id,parseInt(event.target.id),this.itemsOldState)){
        this.notify.showWarn("Zavisnost već postoji", `Zavisnost između taska [${event.source.title}] i taska [${event.target.title}] već postoji.`);
        // log(this.items.find(item => item.id == event.source.id))
        return;
      }else{
        let update: UpdateGant = {
                addDependentOn: [parseInt(event.source.id)], 
              };

              console.log(event.source.id)
              console.log(update);
              this.updateGantItemById(parseInt(event.target.id),update);
              this.getUpdateSuccessObservable().subscribe((success: boolean) => {
                if(success){
                    this.notify.showSuccess("Uspesno dodata zavisnost",`Uspešno dodata zavisnost. [${event.source.title}]->[${event.target?.title}] \n\n Klikom na vezu(liniju) možete obrisati zavisnost.`)

          
                //   let start = new Date(update.startTs*1000);
                //   let end  = new Date(update.endTs*1000 );
                //   const formattedStart = this.datePipe.transform(start, 'medium');
                //   const formattedEnd = this.datePipe.transform(end, 'medium');
                //   this.showSuccess("Uspesno izmenjeno vreme","Promenjeno vreme trajanja taska. \n Pocetak-["+ formattedStart +"]\nKraj-["+ formattedEnd + "]" )
                }else{
                  this.notify.showError("Greška",`Greška pri kreiranju zavisnosti taska  ${event.source.title}`);
                  this.getGanttItemsByProjectId();
                }
              }
            );
        this.notify.showSuccess("USPESNA VEZA", `Zavisnost je kreirana izmedju taska [${event.source.title}] i taska [${event.target.title}].`);
      }
    }




  //   i"Link Drag", `Kada povezemno dva itema Source_id = ${event.source.id} target_id = ${event.target?.id}`)
  }

// Ovo je kada se leva strana pomera na primer listu taskova menjam jedan da dodje iznad drtugog i tako dalje Ovo je kada kliknem
onDragStarted(event: GanttTableDragStartedEvent) {
    // console.log('onDragStarted log', event);
}

// Ovo je kada se leva strana pomera na primer listu taskova menjam jedan da dodje iznad drtugog i tako dalje  ovo je kada pustim klik
onDragEnded(event: GanttTableDragEndedEvent) {
    // console.log('onDragEnded log', event);
}

  print(name: string) {
      this.printService.print(name);
  }

  scrollToToday() {
    this.ganttComponent.scrollToToday();
    }



  // switchChange() {
  //     if (this.isBaselineChecked) {
  //         this.baselineItems = [
  //             { id: '000000', start: 1627728888, end: 1628421197 },
  //             { id: '000001', start: 1617361997, end: 1625483597 },
  //             { id: '000002', start: 1610536397, end: 1610622797 },
  //             { id: '000003', start: 1628507597, end: 1633345997 },
  //             { id: '000004', start: 1624705997 }
  //         ];
  //     } else {
  //         this.baselineItems = [];
  //     }
  // }

  selectView(type: GanttViewType) {
      this.viewType = type;
      this.selectedViewType = type;
  }

  viewChange(event: GanttView) {
      // console.log(event.viewType);
      this.selectedViewType = event.viewType;
  }

  refresh() {
    this.getGanttItemsByProjectId();
  }

  
  downloadGantt(){
    const element = document.getElementById('gantt_id');

    if(!element){
      console.error("Element nije pronadjen");
      return;
    }
    html2Canvas(element).then((canvas) => {
      const imgData = canvas.toDataURL('image/png');
      const link = document.createElement('a');
      link.href = imgData;
      const currentDate: Date = new Date();
      const dateString: string = currentDate.toISOString(); 
      const formattedDate: string = dateString.replace(/[:\-T\.]/g, '').slice(0, -5); 
      const formattedFileName: string = formattedDate.replace('T', '-'); 
      link.download = "gantt-"+formattedFileName+".png";
      link.click();
    })
  }
  

  onDragDropped(event: GanttTableDragDroppedEvent) {
      const sourceItems = event.sourceParent?.children || this.items;
      sourceItems.splice(sourceItems.indexOf(event.source), 1);
      if (event.dropPosition === 'inside') {
          event.target.children = [...(event.target.children || []), event.source];
      } else {
          const targetItems = event.targetParent?.children || this.items;
          if (event.dropPosition === 'before') {
              targetItems.splice(targetItems.indexOf(event.target), 0, event.source);
          } else {
              targetItems.splice(targetItems.indexOf(event.target) + 1, 0, event.source);
          }
      }
      this.items = [...this.items];
  }
  
}