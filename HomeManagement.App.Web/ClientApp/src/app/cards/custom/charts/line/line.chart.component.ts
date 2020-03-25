import { Component, Input } from "@angular/core";
import { Chart } from 'chart.js';
import { PaletteService } from "src/app/services/palette.service";

@Component({
    selector: 'line-chart',
    templateUrl: 'line.chart.component.html'
})
export class LineChartComponent {

    constructor(private paletteService: PaletteService){
        
    }

    canvas: any;
    ctx: any;
    @Input() data: Array<number>;
    @Input() labels: Array<string>;
    @Input() title: string;

    ngAfterViewInit(): void {

        let colors = this.paletteService.getColors(this.data.length);

        this.canvas = document.getElementById('lineChart');
        this.ctx = this.canvas.getContext('2d');

        let myChart = new Chart(this.ctx, {
            type: 'line',
            data: {
                labels: this.labels,
                datasets: [{
                    label: this.title,
                    data: this.data,
                    backgroundColor: colors,
                    fill: 'white',
                    borderColor: '#424242',
                }]
            },
            options: {
                responsive: true,
                display: true
            }
        });
    }
}