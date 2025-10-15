import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ReservationService } from '../../services/reservation.service';

@Component({
  selector: 'app-my-reservations',
  templateUrl: './my-reservations.html',
  styleUrl: './my-reservations.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MyReservationsComponent {
  protected readonly service = inject(ReservationService);
  readonly activeTab = signal<'upcoming' | 'active' | 'past'>('upcoming');
  readonly showCancelDialog = signal(false);
  readonly reservationToCancel = signal<number | null>(null);

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric', year: 'numeric' });
  }

  formatTime(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true });
  }

  calculateDuration(start: string, end: string): string {
    const startDate = new Date(start);
    const endDate = new Date(end);
    const hours = Math.round((endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60));
    return hours === 1 ? '1 hour' : `${hours} hours`;
  }

  openCancelDialog(id: number) {
    this.reservationToCancel.set(id);
    this.showCancelDialog.set(true);
  }

  closeCancelDialog() {
    this.showCancelDialog.set(false);
    this.reservationToCancel.set(null);
  }

  async confirmCancel() {
    const id = this.reservationToCancel();
    if (id !== null) {
      try {
        await this.service.cancelReservation(id);
        this.closeCancelDialog();
      } catch (err) {
        console.error('Failed to cancel reservation:', err);
      }
    }
  }
}
