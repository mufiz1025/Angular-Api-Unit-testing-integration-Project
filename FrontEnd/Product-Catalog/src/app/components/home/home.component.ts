import { Product } from './../../model/product-entity';
import { CommonModule } from '@angular/common';
import { Component,  OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import {  Router, RouterOutlet } from '@angular/router';
import { ProductDataService } from '../../services/product-data.service';
import { Observable } from 'rxjs';



@Component({
  selector: 'app-home',
  imports: [RouterOutlet, FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  public serverData = <Product[]>[];

  constructor(private _productService : ProductDataService , private router :Router 
  ){
  }
  
  ngOnInit(): void {

    this._productService.getData().subscribe(data => { 
      console.log("fetched Data: " , data);
      this.serverData = data;
      
    });
  }
  
  
  onClickView(productId : number | undefined){
   if (productId === undefined )
   {
    console.error('Error : productId is undefined !');
    return;
   }
   console.log("navigating to the productId :" , productId)
    
    this.router.navigate(['/details' ,productId])

    
  }
  
  
  
  onClickEdit(id: number )  {
       if (id === undefined)
       {
        console.error('Error : ProductId is Undefined!');
       }
       console.log('Navigating to the product id ' , id);

       this.router.navigate(['/UpdateProduct' , id]);
     
     
  }
  
    
   
  onClickDelete(productId: number | undefined) {
    if (productId === undefined) {
      console.error('Error: productId is undefined!');
      return;
    }
  
    this._productService.deleteData(productId).subscribe({
      next: () => {
        console.log(`Product with ID ${productId} deleted successfully.`);
        // Optionally refresh the product list
        this._productService.getData().subscribe(data => {
          this.serverData = data;
        });
      },
      error: (err) => {
        console.error('Error deleting product:', err);
      },
    });
  
  }
  
}
