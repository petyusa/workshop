import { Injectable, computed, inject, signal, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { ReservableObject, ReservableObjectListResponse, ObjectType, ObjectTypes } from '../models/reservable-object';
import { LocationService } from './location.service';

@Injectable({
  providedIn: 'root'
})
export class ReservableObjectService {
  private readonly http = inject(HttpClient);
  private readonly locationService = inject(LocationService);
  private readonly apiUrl = 'http://localhost:5074';
  
  readonly objects = signal<ReservableObject[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly filterType = signal<ObjectType>(ObjectTypes.All);
  readonly showAvailableOnly = signal(false);
  readonly viewMode = signal<'grid' | 'floorplan'>('grid');
  
  // Computed filtered objects
  readonly filteredObjects = computed(() => {
    let filtered = this.objects();
    
    // Filter by type
    const type = this.filterType();
    if (type !== ObjectTypes.All) {
      filtered = filtered.filter(obj => obj.type === type);
    }
    
    // Filter by availability
    if (this.showAvailableOnly()) {
      filtered = filtered.filter(obj => obj.isAvailable);
    }
    
    return filtered;
  });
  
  // Stats for display
  readonly stats = computed(() => {
    const all = this.objects();
    return {
      total: all.length,
      available: all.filter(o => o.isAvailable).length,
      desks: all.filter(o => o.type === ObjectTypes.Desk).length,
      parking: all.filter(o => o.type === ObjectTypes.ParkingSpace).length,
    };
  });
  
  constructor() {
    // Auto-load objects when location changes
    effect(() => {
      const locationId = this.locationService.selectedLocationId();
      if (locationId) {
        this.loadObjects(locationId);
      } else {
        this.objects.set([]);
      }
    });
  }
  
  async loadObjects(locationId: number): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    
    try {
      const response = await firstValueFrom(
        this.http.get<ReservableObjectListResponse>(`${this.apiUrl}/api/locations/${locationId}/objects`)
      );
      
      this.objects.set(response?.objects || []);
    } catch (err) {
      console.error('Failed to load reservable objects:', err);
      this.error.set('Failed to load reservable objects');
      this.objects.set([]);
    } finally {
      this.loading.set(false);
    }
  }
  
  setFilterType(type: ObjectType): void {
    this.filterType.set(type);
  }
  
  toggleAvailableOnly(): void {
    this.showAvailableOnly.update(current => !current);
  }
  
  setViewMode(mode: 'grid' | 'floorplan'): void {
    this.viewMode.set(mode);
  }
}
