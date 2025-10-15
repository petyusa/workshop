import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { LocationSelectorComponent } from '../location-selector/location-selector';
import { ReservableObjectsComponent } from '../reservable-objects/reservable-objects';
import { MyReservationsComponent } from '../my-reservations/my-reservations';
import { AccessRequestsComponent } from '../access-requests/access-requests';
import { LocationService } from '../../services/location.service';
import { AccessRequestService } from '../../services/access-request.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [LocationSelectorComponent, ReservableObjectsComponent, MyReservationsComponent, AccessRequestsComponent],
})
export class HomeComponent {
  protected readonly locationService = inject(LocationService);
  protected readonly accessRequestService = inject(AccessRequestService);
  readonly activeTab = signal<'objects' | 'reservations' | 'access-requests'>('objects');
}
