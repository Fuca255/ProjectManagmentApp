// za filtere treba da se zna da flagovi mogu da budu 1 ili -1 i imamo property koji ima vrendnost
// ako je 1 daje sve sto je vece ili jednako vrednosti na koju se odnosi flag, ako je -1 vraca, manje,
// searchTitle property  je case insensitive.
export interface ProjectFilter {
    //ovaj deo je za paginaciju
    pageNumber? : number,
    pageSize? : number,
    //ovaj deo ti je za sortiranje
    propertyName? : string,// title,start,end,..(za detalje vidi Repostories/ProjectRepository.cs 174 linija funkcija)
    sortFlag? : number, // 1 = asc , -1 = desc
    //ovo sve drugo su filteri
    searchTitle? : string,
    startFrom? : Date,
    startTo? : Date,
    endFrom? : Date,
    endTo? : Date,
    priorityFilter? : number[],
    stateFilter? : number[],
    budgetFilterFrom? : number,
    budgetFilterTo? : number,
    spentFilterFrom? : number,
    spentFilterTo? : number,
    percentageFilterFrom? : number,
    percentageFilterTo? : number
}
