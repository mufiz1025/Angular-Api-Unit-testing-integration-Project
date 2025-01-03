import { CommonModule } from '@angular/common';

import { Product } from './../../model/product-entity';

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule,  Validators } from '@angular/forms';
import { RouterLink, RouterOutlet } from '@angular/router';
import { ProductDataService } from '../../services/product-data.service';





@Component({
  selector: 'app-add-product',
  imports: [RouterOutlet , RouterLink ,ReactiveFormsModule , FormsModule , CommonModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.scss'
})
export class AddProductComponent implements OnInit {
 
  

   ProductForm! : FormGroup; 
   
   
   
   public InventoryStatus = ['In Stock' , 'Out Of Stock'];
   
   
   constructor( private fbuilder : FormBuilder , private _dataService : ProductDataService){
      
   }
   get ProductName(){
    return this.ProductForm.get('ProductName');
   }
   get ProductPrice(){
       return this.ProductForm.get('ProductPrice');
   }

  ngOnInit(): void {
    this.ProductForm =this.fbuilder.group({
      ProductName :['' , [Validators.required ,Validators.minLength(3)]],
      ProductPrice :['' , [Validators.required , Validators.min(1)]],
      ProductDescription :[''],
      ProductStatus :['']
    })
  }

 onSubmit(productvalues : Product[]){
    
     this._dataService.setData(productvalues).subscribe(data => productvalues = data);
     console.log(productvalues);
   
  }
}


