import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/api/category.service';
import { Category } from 'src/app/models/category';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'category-list-card',
    templateUrl: './category.list.component.html',
    styleUrls: ['category.list.component.css']
})
export class CategoryListCardComponent implements OnInit {

    categories: Array<Category> = new Array<Category>();
    displayedColumns: string[] = ['isActive', 'measurable', 'icon', 'name'];

    constructor(private categoryService: CategoryService,
        private snackBar: MatSnackBar){
    }

    ngOnInit(): void {
        this.categoryService.getCategories().subscribe(result => {
            result.forEach(c => {
                this.categories.push(c);
            });
        });
    }

    onActiveChanged(category:Category){
        this.updateCategory(category);
    }

    onMeasureChanged(category:Category){
        this.updateCategory(category);
    }

    updateCategory(category: Category){
        console.log(category);
        this.categoryService.update(category).subscribe(result => {
            this.snackBar.open("Category: " + category.name + " updated", "Close", {
                duration: 2000
            })
        });
    }
}