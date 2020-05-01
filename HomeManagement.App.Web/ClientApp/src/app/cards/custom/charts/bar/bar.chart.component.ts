import { Component, Input } from "@angular/core";
import { Chart } from 'chart.js';
import { PaletteService } from "src/app/services/palette.service";

@Component({
    selector: 'bar-chart',
    templateUrl: 'bar.chart.component.html'
})
export class BarChartComponent {

    constructor(private paletteService: PaletteService){
        
    }

    canvas: any;
    ctx: any;
    @Input() data: Array<number>;
    @Input() labels: Array<string>;
    @Input() title: string;

    ngAfterViewInit(): void {

        let colors = this.paletteService.getColors(this.data.length);

        this.canvas = document.getElementById('barChart');
        this.ctx = this.canvas.getContext('2d');

        let myChart = new Chart(this.ctx, {
            type: 'bar',
            data: {
                labels: this.labels,
                datasets: [{
                    label: this.title,
                    data: this.data,
                    backgroundColor: colors,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                display: true
            }
        });
    }
}