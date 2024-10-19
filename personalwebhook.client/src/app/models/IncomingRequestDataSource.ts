import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { IncomingRequestBase} from './IncomingRequestBase';

export class IncomingRequestDataSource extends MatTableDataSource<IncomingRequestBase> {

    private collection: IncomingRequestBase[] = [];

    private collection$: Subscription;

    constructor(collection: Observable<IncomingRequestBase>) {
        super();
        this.collection$ = collection.subscribe(data => {
            this.collection.unshift(data);
            this.data = this.collection;
        });
    }

   override disconnect() {
     this.collection$.unsubscribe();
     super.disconnect();
   }
}