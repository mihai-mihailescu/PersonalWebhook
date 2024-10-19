import { Injectable } from '@angular/core';
import { SignalRService } from './signalR.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WebhookService {
  constructor(private readonly signalRService: SignalRService, private readonly httpClient: HttpClient) {
  }

  getSessionId(): Observable<string> {
    return this.httpClient.get('/Webhook/GetWebhookDetails', { responseType: 'text' })
  }

  getBaseUrl(): Observable<string> {
    return this.httpClient.get('/Webhook/GetBaseUrl', { responseType: 'text' })
  }
}
