import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  confirm(title = 'Confirmation', 
    message='Are you sure you want to do this?', 
    btnOkTxt = 'Ok', 
    btnCancelTxt = 'Cancel'): Observable<boolean> {
      const config = {
        initialState: {
          title,
          message,
          btnOkTxt,
          btnCancelTxt
        }
      }

    this.bsModalRef = this.modalService.show(ConfirmDialogComponent, config);
    
    return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (observer) => {
      const subscription = this.bsModalRef.onHidden.subscribe(() => {
        observer.next(this.bsModalRef.content.result);
        observer.complete();
      });

      return {
        unsubscribe() {
          subscription.unsubscribe();
        }
      }
    }
  }
}
