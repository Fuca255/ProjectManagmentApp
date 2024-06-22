import { AfterViewInit, Directive, ElementRef, HostListener, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appMyTooltip]'
})
export class MyTooltipDirective{
  // private tooltip: HTMLElement | null = null;

  constructor(private elementRef: ElementRef){}
  @HostListener('mouseenter')
  onMouseEnter(): void {
    setTimeout(() => {
      const element = this.elementRef.nativeElement;
      if (element.offsetWidth < element.scrollWidth) {
        element.title = element.textContent;
      }
      else if (element.title) element.removeAttribute('title'); 
    }, 500);
  }
}