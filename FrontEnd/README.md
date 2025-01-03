# Angular_Internship_Task



Product Catalog App
Create a simple Product Catalog App with the following features:
 
Requirements:
1.Home Page:
 
A table view of product fetched from a service.
Each product card should display:
Product Name
Price
"View Details" button.
 
2.Product Details Page:
Displays the details of a single product, including:
Name
Price
Description
Stock Status (In Stock/Out of Stock).
A "Back to Catalog" button to navigate back to the Home Page.
 
3.Add Product Form:
A separate page with a form to add new products with fields:
Name (required, min-length 3).
Price (required, positive number).
Description (optional).
Stock Status (dropdown with options "In Stock" and "Out of Stock").
On submission, the product gets added to the catalog, and the user is redirected back to the Home Page.
 
4.Routing:
Configure routes for:
Home Page
Product Details Page (with product ID)
Add Product Page.
 
5.Data Storage:
Use a service to store and manage product data.
 
6.Conditional Directives:
Display "No Products Available" if there are no products in the catalog.
Highlight products that are out of stock.
