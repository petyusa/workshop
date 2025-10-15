import { ChangeDetectionStrategy, Component, input, output, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccessRequestService } from '../../services/access-request.service';
import { ReservableObject } from '../../models/reservable-object';

@Component({
  selector: 'app-access-request-dialog',
  imports: [FormsModule],
  templateUrl: './access-request-dialog.html',
  styleUrls: ['./access-request-dialog.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccessRequestDialogComponent {
  private readonly accessRequestService = inject(AccessRequestService);

  readonly reservableObject = input.required<ReservableObject>();
  readonly onSuccess = output<void>();
  readonly onClose = output<void>();

  readonly requestedStartTime = signal('');
  readonly requestedEndTime = signal('');
  readonly message = signal('');
  readonly error = signal<string | null>(null);
  readonly submitting = signal(false);

  constructor() {
    const now = new Date();
    now.setHours(now.getHours() + 1, 0, 0, 0);
    this.requestedStartTime.set(this.formatDateTime(now));

    const end = new Date(now);
    end.setHours(end.getHours() + 2);
    this.requestedEndTime.set(this.formatDateTime(end));
  }

  private formatDateTime(date: Date): string {
    return date.toISOString().slice(0, 16);
  }

  async submit() {
    this.error.set(null);
    this.submitting.set(true);

    const start = new Date(this.requestedStartTime());
    const end = new Date(this.requestedEndTime());

    if (end <= start) {
      this.error.set('End time must be after start time');
      this.submitting.set(false);
      return;
    }

    const now = new Date();
    if (start < now) {
      this.error.set('Start time must be in the future');
      this.submitting.set(false);
      return;
    }

    try {
      await this.accessRequestService.createRequest({
        reservableObjectId: this.reservableObject().id,
        requestedStartTime: start.toISOString(),
        requestedEndTime: end.toISOString(),
        message: this.message() || undefined,
      });

      this.onSuccess.emit();
    } catch (err: any) {
      this.error.set(err.message || 'Failed to submit request');
    } finally {
      this.submitting.set(false);
    }
  }

  close() {
    this.onClose.emit();
  }

  setDuration(hours: number) {
    const start = new Date(this.requestedStartTime());
    const end = new Date(start);
    end.setHours(end.getHours() + hours);
    this.requestedEndTime.set(this.formatDateTime(end));
  }
}
