import { Injectable } from '@angular/core';
import { SignalRService } from './signalR.service';

@Injectable({
  providedIn: 'root'
})
export class WebhookService {

  constructor(private signalRService: SignalRService) { }

  startup(){
    // this.signalRService.startConnection().subscribe(() => {
    //   this.signalRService.receiveMessage().subscribe((message) => {
    //     this.receivedMessage = message;
    //   });
    // });
  }
}
