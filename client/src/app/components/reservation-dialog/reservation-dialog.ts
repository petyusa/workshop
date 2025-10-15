import { ChangeDetectionStrategy, Component, EventEmitter, inject, input, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReservableObject } from '../../models/reservable-object';
import { ReservationService } from '../../services/reservation.service';
import { CreateReservationRequest } from '../../models/reservation';

@Component({
  selector: 'app-reservation-dialog',
  imports: [FormsModule],
  templateUrl: './reservation-dialog.html',
  styleUrl: './reservation-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservationDialogComponent {
  private readonly reservationService = inject(ReservationService);
  
  readonly reservableObject = input.required<ReservableObject>();
  
  @Output() readonly onSuccess = new EventEmitter<void>();
  @Output() readonly onClose = new EventEmitter<void>();

  readonly startTime = signal('');
  readonly endTime = signal('');
  readonly selectedDuration = signal<number | 'custom'>(2);
  readonly validationError = signal<string | null>(null);
  readonly submitting = signal(false);

  ngOnInit() {
    const now = new Date();
    now.setMinutes(Math.ceil(now.getMinutes() / 60) * 60);
    now.setSeconds(0);
    now.setMilliseconds(0);
    
    const startDateTime = new Date(now.getTime() + 60 * 60 * 1000);
    this.startTime.set(this.formatDateTime(startDateTime));
    this.updateEndTime();
  }

  updateEndTime() {
    const duration = this.selectedDuration();
    if (duration === 'custom') return;

    const start = new Date(this.startTime());
    if (!isNaN(start.getTime())) {
      const end = new Date(start.getTime() + duration * 60 * 60 * 1000);
      this.endTime.set(this.formatDateTime(end));
    }
  }

  onStartTimeChange() {
    if (this.selectedDuration() !== 'custom') {
      this.updateEndTime();
    }
    this.validationError.set(null);
  }

  onDurationChange() {
    this.updateEndTime();
    this.validationError.set(null);
  }

  async reserve() {
    this.validationError.set(null);

    if (!this.startTime() || !this.endTime()) {
      this.validationError.set('Please select start and end times');
      return;
    }

    const start = new Date(this.startTime());
    const end = new Date(this.endTime());

    if (start >= end) {
      this.validationError.set('End time must be after start time');
      return;
    }

    const now = new Date();
    if (start < now) {
      this.validationError.set('Start time must be in the future');
      return;
    }

    const durationHours = (end.getTime() - start.getTime()) / (1000 * 60 * 60);
    const maxDuration = this.reservableObject().type === 'Desk' ? 8 : 12;
    if (durationHours > maxDuration) {
      this.validationError.set(`Duration cannot exceed ${maxDuration} hours for ${this.reservableObject().type}`);
      return;
    }

    this.submitting.set(true);

    try {
      const request: CreateReservationRequest = {
        reservableObjectId: this.reservableObject().id,
        startTime: start.toISOString(),
        endTime: end.toISOString(),
      };

      const conflictCheck = await this.reservationService.checkAvailability(request);
      
      if (conflictCheck.hasConflict) {
        this.validationError.set(conflictCheck.message || 'Time slot unavailable');
        this.submitting.set(false);
        return;
      }

      await this.reservationService.createReservation(request);
      this.onSuccess.emit();
    } catch (err: any) {
      this.validationError.set(err.message || 'Failed to create reservation');
    } finally {
      this.submitting.set(false);
    }
  }

  close() {
    this.onClose.emit();
  }

  private formatDateTime(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }
}
