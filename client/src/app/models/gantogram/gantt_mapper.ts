import { GanttGroup, GanttGroupInternal, GanttItem, GanttItemType, GanttLink, GanttLinkType, } from "@worktile/gantt";
import { TaskCardKanbanComponent } from "../../components/task-card-kanban/task-card-kanban.component";
import { Task } from "../task/task";
import { Priority } from "../priority/priority";
import { GantogramService } from "../../services/gantogram.service"
import { log } from "node:console";
import { TaskGroup } from "./taskGroup";
import { Id } from "ngx-tethys/types";
import { group } from "@angular/animations";
export class GanttMapper {
    // Normalni članovi klase
    static mapTasksToGantItems(tasks:Task[],ganttGroups: GanttGroup[]) : GanttItem[]{
        // const ganttGroups : GanttGroup[] = []
        const ganttItems: GanttItem[] = []        
        for (let i = 0; i < tasks.length; i++) {         
            ganttItems.push(this.mapTaskToGanttItem(tasks[i],ganttGroups));
            console.log((ganttGroups));
            
        }
        console.log(ganttItems);
        
        return ganttItems
    }

    static mapTaskToGanttItem(task: Task, groups:GanttGroup[]): GanttItem {
        const ganttItem: GanttItem = {
            id: task.id.toString(),
            title: task.title + " (" + task.percentage + "%)",
            start: Math.floor(new Date(task.start).getTime()/1000), // ili drugi odgovarajući atribut za početak
            end: Math.floor(new Date(task.end).getTime()/1000), // ili drugi odgovarajući atribut za kraj
            links: this.convertToGantLinks(task.depndentOn), // Možete dodati logiku za mapiranje linkova ako je potrebno
            draggable: true, // Postavite na true ako želite omogućiti povlačenje
            itemDraggable:  true, // Postavite na true ako želite omogućiti povlačenje samo na ovom elementu
            linkable: true, // Postavite na true ako želite omogućiti dodavanje linkova
            expandable: false, // Postavite na true ako želite omogućiti proširivanje
            expanded: false, // Postavite na true ako želite da je element početno proširen
            color: this.mapPrioprityToColor(task.priority), // Postavite boju po želji
            
            group_id : this.addTaskGroup(task.taskGroup,groups),
            // barStyle: {color: 'red !important'}, // Postavite stil trake po želji
            origin: task, // Originalni task
            type: GanttItemType.bar, // Tip gant elementa
            progress: task.percentage/100, // Napredak elementa ako je dostupno u tasku
        };
        return ganttItem;
        
    }

    static addTaskGroup(group: TaskGroup, groups: GanttGroup[]): string{
       
        
        if(!group.parentTaskGroupId){
            const existingGroup = groups.find(g => g.id === "0");
            if(!existingGroup){
                const newGanttGroup: GanttGroup<TaskGroup> = {
                    id: "0", // Pretvaranje broja u string
                    title: group.title,
                    // origin: group
                };
                groups.push(newGanttGroup);
                return newGanttGroup.id
            }else{
                return existingGroup.id;
            }
        }else{
            const existingGroup = groups.find(g => g.id === group.id.toString());
    
            if (existingGroup) {
                // Ako postoji, vrati id
                return existingGroup.id;
            } else {
                // Ako ne postoji, kreiraj novi GanttGroup objekat
                const newGanttGroup: GanttGroup<TaskGroup> = {
                    id: group.id.toString(), // Pretvaranje broja u string
                    title: "Podgrupa: " + group.title,
                    // origin: group
                };
                
                // Dodaj novi GanttGroup objekat u niz
                groups.push(newGanttGroup);
                
                // Vrati id novododate grupe
                return newGanttGroup.id;
            }
        }

        
        // Provera da li već postoji grupa sa istim id-jem
    }


    // Logika na osnovu role da li je korisniku dozvoljeno da menja vreme start-end draggable: (true/false)
    private static convertToGantLinks(array: number[]): GanttLink[] {
        return array.map(number => ({
            type: 1, // Modulo operacija garantuje da se type vrednosti kreću od 1 do 4
            link: number.toString()
          }));
    }

    private static mapPrioprityToColor(priority:Priority):string{
        switch(priority.level){
            case  1: return "#06d6a0" // low;
            case  2: return "#118ab2" // Medium;
            case  3: return "#ffd166" // High;
            case  4: return "#ef476f" // AtRisk;
            default: return "#dc3545" // default
        }
    }

    static checkIfNumberExists(id: string, numberToCheck: number, items: GanttItem[]): boolean {
        // Pronalazimo GanttItem sa odgovarajućim id
        const item = items.find(item => item.id === id);

        // Proveravamo da li smo pronašli GanttItem sa datim id
        if (item && item.links) {
            console.log("Nasao sam item");
            if (item.links.some(link => link.link === numberToCheck.toString())) {
                return true; // Ako postoji, vraćamo true
            }
            
        }
      
        return false; // Ako ne postoji ili nije pronađen odgovarajući GanttItem, vraćamo false
      }

      static removeLink(id: string, linkToRemove: string, items: GanttItem[]): GanttItem[] {
        // Pronalazimo GanttItem sa odgovarajućim ID-om
        const item = items.find(item => item.id === id);
    
        // Proveravamo da li smo pronašli GanttItem sa datim ID-om i da li ima linkove
        if (item && item.links) {
            // Pronalazimo indeks linka koji želimo da uklonimo
            const linkIndex = item.links.findIndex(link => link.link === linkToRemove);
            
            // Ako je link pronađen, uklanjamo ga iz liste linkova
            if (linkIndex !== -1) {
                item.links.splice(linkIndex, 1);
                console.log(item);
                console.log(items);
                
                return items; // Vraćamo true ako je link uspešno uklonjen
            }
        }
      
        return items; // Vraćamo false ako nije pronađen link ili GanttItem sa datim ID-om
    }


} 