import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReservableObjectService } from '../../services/reservable-object.service';
import { ObjectTypes } from '../../models/reservable-object';

@Component({
  selector: 'app-reservable-objects',
  templateUrl: './reservable-objects.html',
  styleUrls: ['./reservable-objects.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservableObjectsComponent {
  protected readonly service = inject(ReservableObjectService);
  protected readonly ObjectTypes = ObjectTypes;
}
