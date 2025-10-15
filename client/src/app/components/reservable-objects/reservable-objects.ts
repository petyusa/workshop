import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ReservableObjectService } from '../../services/reservable-object.service';
import { ReservationService } from '../../services/reservation.service';
import { AuthService } from '../../services/auth.service';
import { ObjectTypes, ReservableObject } from '../../models/reservable-object';
import { ReservationDialogComponent } from '../reservation-dialog/reservation-dialog';
import { AccessRequestDialogComponent } from '../access-request-dialog/access-request-dialog';

@Component({
  selector: 'app-reservable-objects',
  imports: [ReservationDialogComponent, AccessRequestDialogComponent],
  templateUrl: './reservable-objects.html',
  styleUrls: ['./reservable-objects.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservableObjectsComponent {
  protected readonly service = inject(ReservableObjectService);
  protected readonly reservationService = inject(ReservationService);
  protected readonly authService = inject(AuthService);
  protected readonly ObjectTypes = ObjectTypes;

  readonly showDialog = signal(false);
  readonly showAccessRequestDialog = signal(false);
  readonly selectedObject = signal<ReservableObject | null>(null);

  openReservationDialog(object: ReservableObject) {
    this.selectedObject.set(object);
    this.showDialog.set(true);
  }

  openAccessRequestDialog(object: ReservableObject) {
    this.selectedObject.set(object);
    this.showAccessRequestDialog.set(true);
  }

  closeDialog() {
    this.showDialog.set(false);
    this.showAccessRequestDialog.set(false);
    this.selectedObject.set(null);
  }

  async onReservationSuccess() {
    this.closeDialog();
    const locationId = this.service['locationService'].selectedLocationId();
    if (locationId) {
      await this.service.loadObjects(locationId);
    }
  }

  async onAccessRequestSuccess() {
    this.closeDialog();
  }

  hasActiveReservation(objectId: number): boolean {
    return this.reservationService.hasActiveReservation(objectId);
  }

  isOwnedByOther(object: ReservableObject): boolean {
    const currentUser = this.authService.username();
    return !!object.ownerUsername && object.ownerUsername !== currentUser;
  }
}
