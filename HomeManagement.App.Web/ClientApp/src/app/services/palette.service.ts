import { Injectable } from "@angular/core";

@Injectable()
export class PaletteService {

    private colors: Array<string> = [
        "#00a8cc",
        "#ffaaa5",
        "#ffd31d",
        "#00bdaa",
        "#cae8d5",
        "#639a67",
        "#ff1e56",
        "#7d5e2a",
        "#ffa34d",
        "#424874",
        "#fafba4",
        "#216353",
        "#7a4d1d",
        "#1b262c",
        "#e8f044",
        "#5b8c5a",
        "#b21f66",
        "#8ac6d1",
        "#b7472a",
        "#50bda1",
        "#c06c84",
        "#9818d6",
        "#c81912"
    ];

    getColors(limit:number): Array<string>{
        return this.colors.slice(0, limit);        
    }
}