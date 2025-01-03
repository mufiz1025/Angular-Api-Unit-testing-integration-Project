import { Product } from './../model/product-entity';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProductDataService {
   
   constructor(private http: HttpClient){}

   private _url :string ="http://localhost:5202/api/Products";
 
   getData() : Observable<Product[]>{
     return   this.http.get<Product[]>(this._url);

  }
   getDatabyId(id :number ) : Observable<Product> {
      return this.http.get<Product>(`${this._url}/${id}`).pipe
      (catchError(() =>{
        return of(new Product());
      })
    );
   }
  

  setData(productValues : Product[]) : Observable<Product[]> {
    
    return  this.http.post<Product[]>(this._url , productValues);
   
  }
 
  updateData(productValues : Product , id : number) : Observable<Product>{

     return this.http.put<Product>(`${this._url}/${id}`, productValues).pipe(
      catchError((error) => {
        console.error('Update Failed', error);
        return throwError(() => new Error('Update Operation failed!'))
      })
     );
  }

  deleteData(id : number) : Observable<void>{
    return this.http.delete<void>(`${this._url}?id=${id}`).pipe(
      catchError((error) => {
        console.error('Delete failed', error);
        return throwError(() => new Error('Delete operation failed'));
      })
    );
  }
}
