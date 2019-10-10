import { Directive, HostListener, AfterViewInit, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appDynamicHeight]'
})
export class DynamicHeightDirective implements AfterViewInit {

  @Input() appDynamicHeight;

  constructor(private el: ElementRef) {
  }

  ngAfterViewInit(): void {
    this.calculateAndSetElementHeight();
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    this.calculateAndSetElementHeight();
  }

  private calculateAndSetElementHeight(): void {
    const mainElement = document.getElementsByClassName('app-body')[0] as HTMLElement;
    const footerElement = document.getElementsByClassName('footer') && document.getElementsByClassName('footer')[0] as HTMLElement;
    const windowHeight = mainElement ? mainElement.offsetHeight : 0;
    const footerHeight = footerElement ? footerElement.offsetHeight : 0;
    const parentOffset = this.el.nativeElement.offsetParent.offsetTop;
    const elementOffsetTop = this.el.nativeElement.offsetTop;
    const height = windowHeight - footerHeight - elementOffsetTop;
    if (this.appDynamicHeight)
      this.el.nativeElement.style.minHeight = `${(height - parentOffset)}px`;
    else
      this.el.nativeElement.style.height = `${height}px`;


    // this.el.nativeElement.style.height = this.appDynamicHeight ? `${(height - parentOffset)}px` : `${height}px`;
  }

}
