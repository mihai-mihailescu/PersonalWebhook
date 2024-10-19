export interface IncomingRequestBase {
  method: string;
  requestId: string;
  sessionId: string;
  requestBody: string;
  date: Date;
  headers: Array<DictionaryKeyValues>;
  absoluteUri: string;
  queryString: string;
  remoteIpAddress: string;
  remotePort: number;
}

export interface DictionaryKeyValues{
  key: string;
  value: string;
}
