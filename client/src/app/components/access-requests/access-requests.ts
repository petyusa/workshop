import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { AccessRequestService } from '../../services/access-request.service';
import { AccessRequest } from '../../models/access-request';

@Component({
  selector: 'app-access-requests',
  imports: [],
  templateUrl: './access-requests.html',
  styleUrls: ['./access-requests.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccessRequestsComponent {
  protected readonly service = inject(AccessRequestService);

  readonly showRespondDialog = signal(false);
  readonly selectedRequest = signal<AccessRequest | null>(null);
  readonly responseMessage = signal('');
  readonly responding = signal(false);

  openRespondDialog(request: AccessRequest) {
    this.selectedRequest.set(request);
    this.responseMessage.set('');
    this.showRespondDialog.set(true);
  }

  closeRespondDialog() {
    this.showRespondDialog.set(false);
    this.selectedRequest.set(null);
    this.responseMessage.set('');
  }

  async respond(approved: boolean) {
    const request = this.selectedRequest();
    if (!request) return;

    this.responding.set(true);
    
    try {
      await this.service.respondToRequest(request.id, {
        status: approved ? 'Approved' : 'Denied',
        responseMessage: this.responseMessage() || undefined,
      });

      this.closeRespondDialog();
    } catch (err) {
      console.error('Failed to respond to request:', err);
    } finally {
      this.responding.set(false);
    }
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleString();
  }

  formatDuration(startStr: string, endStr: string): string {
    const start = new Date(startStr);
    const end = new Date(endStr);
    const hours = Math.abs(end.getTime() - start.getTime()) / 36e5;
    
    if (hours < 1) {
      return `${Math.round(hours * 60)} minutes`;
    }
    return `${hours.toFixed(1)} hours`;
  }
}
