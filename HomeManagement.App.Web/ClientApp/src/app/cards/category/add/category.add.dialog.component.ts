import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material";
import { Validators, FormControl } from "@angular/forms";
import { FormError } from "src/app/models/base-types";
import { Category } from "src/app/models/category";
import { MaterialIcon, getMaterialIcons } from "src/app/common/material.icons";
import { startWith, map } from "rxjs/operators";
import { Observable } from "rxjs";

@Component({
    selector: 'category-add-dialog',
    templateUrl: 'category.add.dialog.component.html'
})
export class CategoryAddDialog implements OnInit{

    nameFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);

    iconFormControl: FormControl = new FormControl('', [
        Validators.required
    ]);
    
    errors: Array<FormError> = new Array<FormError>();

    icons: Array<MaterialIcon> = new Array<MaterialIcon>();
    filteredIcons: Observable<Array<string>>;
    category: Category = new Category();
    
    constructor(
        public dialogRef: MatDialogRef<CategoryAddDialog>,
        private snackBar: MatSnackBar) {

            this.errors.push(new FormError('Name is required', this.nameFormControl));
            this.errors.push(new FormError('Icon is required', this.iconFormControl));
        }


    ngOnInit(): void {

        this.icons = getMaterialIcons();

        this.filteredIcons = this.iconFormControl.valueChanges

        .pipe(startWith(''),
                map(value => this.filterIcons(value)));

        this.iconFormControl.valueChanges.subscribe(icon => {
            this.category.icon = icon;
        });
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    ok(){
        for (let index = 0; index < this.errors.length; index++) {
            const element = this.errors[index];

            if(!element.control.valid){
                this.snackBar.open(element.message, 'Close',{
                    duration: 2000
                });
    
                return;
            }          
        }

        this.dialogRef.close(this.category);
    }

    private filterIcons(iconValue:any) : Array<string> {

        var filteredValues = this.icons            
            .filter(option => {
                let selected = option.value.toLowerCase().includes(iconValue);
                
                return selected;
            })
            .map(c => c.value);

        return filteredValues;
    }
}