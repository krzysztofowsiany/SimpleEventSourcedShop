import {Component, Inject} from '@angular/core';
import * as uuid from 'uuid';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  orderId:string;
  productId:string;
  clientId:string;

  constructor(private httpClient: HttpClient,  @Inject('BASE_URL') private baseUrl: string) {
    this.generateId();
    this.generateProductId();
    this.generateClientId();
  }

  generateId(){
    this.orderId = uuid.v4();
  }

  generateProductId(){
    this.productId = uuid.v4();
  }

  generateClientId(){
    this.clientId = uuid.v4();
  }


  CreateOrder() {
    this.httpClient
      .post(`${this.baseUrl}api/order/create`, {
        OrderId: this.orderId
      })
      .subscribe(result => {
        console.log(result);
        },
          error => console.error(error));
  }

  AddProduct() {
    this.httpClient
      .post(`${this.baseUrl}api/order/add`, {
        OrderId: this.orderId,
        ProductId: this.productId
      })
      .subscribe(result => {
          console.log(result);
        },
        error => console.error(error));
  }

  RemoveProduct() {
    this.httpClient
      .post(`${this.baseUrl}api/order/remove`, {
        OrderId: this.orderId,
        ProductId: this.productId
      })
      .subscribe(result => {
          console.log(result);
        },
        error => console.error(error));
  }

  ApplyClient() {
    this.httpClient
      .post(`${this.baseUrl}api/order/apply`, {
        OrderId: this.orderId,
        ClientId: this.clientId
      })
      .subscribe(result => {
          console.log(result);
        },
        error => console.error(error));
  }
}
