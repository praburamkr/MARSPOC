import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[bgColor]'
})
export class BGDirective {

  constructor(private el: ElementRef) { }

  @Input('bgColor') bkgColor: string;

  @HostListener('click') onMouseClick() {
    this.changeBG(this.bkgColor || 'red');
  }

  private changeBG(color: string) {
    this.el.nativeElement.style.backgroundColor = color;
  }
}