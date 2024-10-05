import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { IncomingRequestBase } from '../models/IncomingRequestBase';
@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7019/webhookHub',
        {
          skipNegotiation: false,
          transport: signalR.HttpTransportType.WebSockets
        }
      ) // SignalR hub URL
      .build();
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  receiveMessage(): Observable<IncomingRequestBase> {
    return new Observable<IncomingRequestBase>((observer) => {
      this.hubConnection.on('ReceivedRequest', (message: IncomingRequestBase) => {
        observer.next(message);
      });
    });
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message);
  }
}
