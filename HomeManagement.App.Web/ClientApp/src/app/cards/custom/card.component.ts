import { Component, Input } from '@angular/core';

@Component({
  selector: 'content-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class ContentCardComponent {  
    @Input() title: string;
}
