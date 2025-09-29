import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class App {
  protected readonly title = signal('Sonrisa');
  protected readonly subtitle = signal('Smart Workspace Solutions');
  protected readonly availableDesks = signal(42);
  protected readonly availableParking = signal(28);
  protected readonly totalEmployees = signal(156);
  protected readonly activeBookings = signal(87);
  
  // Computed values for dashboard stats
  protected readonly deskUtilization = computed(() => 
    Math.round(((this.totalEmployees() - this.availableDesks()) / this.totalEmployees()) * 100)
  );
  
  protected readonly parkingUtilization = computed(() => 
    Math.round(((50 - this.availableParking()) / 50) * 100)
  );

  // Demo actions
  protected bookDesk() {
    this.availableDesks.update(current => Math.max(0, current - 1));
  }

  protected bookParking() {
    this.availableParking.update(current => Math.max(0, current - 1));
  }

  protected viewMyBookings() {
    // Navigate to bookings view (placeholder)
    console.log('Navigate to my bookings');
  }
}
