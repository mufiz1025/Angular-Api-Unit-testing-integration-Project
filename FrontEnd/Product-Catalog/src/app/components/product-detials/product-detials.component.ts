import { Product } from './../../model/product-entity';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';

import { ProductDataService } from '../../services/product-data.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-product-detials',
  imports: [RouterOutlet, FormsModule , CommonModule, RouterLink ],
  templateUrl: './product-detials.component.html',
  styleUrl: './product-detials.component.scss'
})
export class ProductDetialsComponent implements OnInit {

   
   public productid? : number ;
   public product? : Product;
   public status?: string ;
 
 constructor( private _dataservice : ProductDataService, private route:ActivatedRoute){
     
 }
 public errorMSG: any;
 public serverData = <Product[]>[];
 
 ngOnInit(): void {
    let id = parseInt(this.route.snapshot.paramMap.get('id')!);
    console.log(id);
    this._dataservice.getDatabyId(id).subscribe(data => this.product = data);
    this.productid = id ;

    this._dataservice.getData()
     .subscribe(data => this.serverData = data    
     );
 }

 
}
