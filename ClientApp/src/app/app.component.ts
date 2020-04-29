import { Component, Inject } from '@angular/core';
import * as uuid from 'uuid';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  orderId: string;
  productId: string;
  clientId: string;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.generateId();
    this.generateProductId();
    this.generateClientId();
  }

  generateId() {
    this.orderId = uuid.v4();
  }

  generateProductId() {
    this.productId = uuid.v4();
  }

  generateClientId() {
    this.clientId = uuid.v4();
  }

  CreateOrder() {
    this.sendRequest("api/order/create", {
      OrderId: this.orderId
    });
  }

  AddProduct() {
    this.sendRequest("api/order/add", {
      OrderId: this.orderId,
      ProductId: this.productId
    });
  }

  RemoveProduct() {
    this.sendRequest("api/order/remove", {
      OrderId: this.orderId,
      ProductId: this.productId
    });
  }

  ApplyClient() {
    this.sendRequest("api/order/apply", {
      OrderId: this.orderId,
      ClientId: this.clientId
    });
  }

  Confirm() {
    this.sendRequest("api/order/confirm", {
      OrderId: this.orderId
    });
  }

  sendRequest(url: String, body: any) {
    this.httpClient
      .post(`${this.baseUrl}${url}`, body)
      .subscribe(result => {
        console.log(result);
      },
        error => console.error(error));
  }

}
