import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalR.service';
import { IncomingRequestBase } from './models/IncomingRequestBase';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  receivedMessages: Array<IncomingRequestBase> = new Array<IncomingRequestBase>;
  receivedRequestBody: Array<string | undefined> = new Array<string>;

  testMessage : IncomingRequestBase = {
    Method: 'POST',
    RequestBody: 'BODY IS HERE',
    SessionId: '123',
    RequestId: '111'
  };

  constructor(private http: HttpClient, private signalRService: SignalRService) {}

  ngOnInit() {
    this.startup();
  }

  startup(){
    this.signalRService.startConnection().subscribe(() => {
      this.signalRService.receiveMessage().subscribe((message) => {
        let res = message
        this.receivedMessages?.unshift(res);
        this.receivedRequestBody?.unshift(res.RequestBody)
        console.log(res)
      });
    });
  }
  
  sendMessage(message: string): void {
    this.signalRService.sendMessage(message);
  }
  
  title = 'Personal Webhook';
}
