import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

// import { MatList } from '@angular/material/list';

import { IncomingRequestDataSource } from './models/IncomingRequestDataSource';
import { IncomingRequestBase } from './models/IncomingRequestBase';

import { SignalRService } from './services/signalR.service';
import { WebhookService } from './services/webhook.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Personal Webhook';

  baseUrl!: string;
  selectedMessage!: IncomingRequestBase | null;
  displayedColumns: string[] = [];
  sessionId!: string;
  selectedRowIndex: number = 0
  dataSource!: IncomingRequestDataSource

  constructor(private readonly http: HttpClient,
    private readonly signalRService: SignalRService,
    private readonly webhookService: WebhookService) { }

  ngOnInit() {
    this.startup();
    this.initializeTableColumns();
  }

  private initializeTableColumns() {
    this.displayedColumns = ["method", "requestId", "date"]
  }

  onRowSelect(row: IncomingRequestBase) {
    this.selectedMessage = this.selectedMessage == row ? null : row;
  }

  private startup() {
    this.webhookService.getSessionId().subscribe(res => {
      this.sessionId = res
    });

    this.webhookService.getBaseUrl().subscribe(res => {
      this.baseUrl = res + '/webhook/' + this.sessionId
    });

    this.signalRService.startConnection().subscribe(() => {
      const observableData$: Observable<IncomingRequestBase> = this.signalRService.receiveMessage();
      this.dataSource = new IncomingRequestDataSource(observableData$);
    });

  }

  sendMessage(message: string): void {
    this.signalRService.sendMessage(message);
  }


}
