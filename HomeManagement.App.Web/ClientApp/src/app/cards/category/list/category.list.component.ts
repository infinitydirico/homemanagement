import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/api/main/category.service';
import { Category } from 'src/app/models/category';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CategoryAddDialog } from '../add/category.add.dialog.component';
import { ColorService } from 'src/app/services/color.service';

@Component({
    selector: 'category-list-card',
    templateUrl: './category.list.component.html',
    styleUrls: ['category.list.component.css']
})
export class CategoryListCardComponent implements OnInit {

    categories: Array<Category> = new Array<Category>();
    displayedColumns: string[] = ['isActive', 'measurable', 'icon', 'name', 'delete'];

    constructor(private categoryService: CategoryService,
        private snackBar: MatSnackBar,
        public dialog: MatDialog,
        private colorService: ColorService) {
    }

    ngOnInit(): void {
        this.categories.splice(0, this.categories.length);

        this.categoryService.getCategories().subscribe(result => {
            result.forEach(c => {
                this.categories.push(c);
            });
        });
    }

    onActiveChanged(category: Category) {
        this.updateCategory(category);
    }

    onMeasureChanged(category: Category) {
        this.updateCategory(category);
    }

    updateCategory(category: Category) {
        console.log(category);
        this.categoryService.update(category).subscribe(result => {
            this.snackBar.open("Category: " + category.name + " updated", "Close", {
                duration: 2000
            })
        });
    }

    add() {
        let categoryDialog = this.dialog.open(CategoryAddDialog, {
            width: '250px'
        })

        categoryDialog.afterClosed().subscribe(category => {

            if (category === undefined) return;

            this.categoryService.add(category).subscribe(result => {
                this.ngOnInit();
            });
        });
    }

    delete(category: Category){
        this.categoryService.delete(category).subscribe(r => this.ngOnInit());
    }
}