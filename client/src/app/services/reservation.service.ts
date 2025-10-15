import { HttpClient } from '@angular/common/http';
import { Injectable, computed, effect, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import type { Reservation, CreateReservationRequest, ConflictCheck } from '../models/reservation';
import { LocationService } from './location.service';

@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  private readonly http = inject(HttpClient);
  private readonly locationService = inject(LocationService);
  private readonly apiUrl = 'http://localhost:5074/api/reservations';

  readonly myReservations = signal<Reservation[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly upcomingReservations = computed(() => {
    const now = new Date();
    return this.myReservations()
      .filter(r => new Date(r.startTime) > now && r.status === 'Active')
      .sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime());
  });

  readonly activeReservations = computed(() => {
    const now = new Date();
    return this.myReservations()
      .filter(r => {
        const start = new Date(r.startTime);
        const end = new Date(r.endTime);
        return now >= start && now <= end && r.status === 'Active';
      });
  });

  readonly pastReservations = computed(() => {
    const now = new Date();
    return this.myReservations()
      .filter(r => new Date(r.endTime) < now || r.status !== 'Active')
      .sort((a, b) => new Date(b.startTime).getTime() - new Date(a.startTime).getTime());
  });

  constructor() {
    effect(() => {
      const locationId = this.locationService.selectedLocationId();
      if (locationId) {
        this.loadMyReservations();
      }
    });
  }

  async createReservation(request: CreateReservationRequest): Promise<Reservation> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await firstValueFrom(
        this.http.post<Reservation>(this.apiUrl, request)
      );
      await this.loadMyReservations();
      return response;
    } catch (err: any) {
      const errorMsg = err.error?.message || err.error || 'Failed to create reservation';
      this.error.set(errorMsg);
      throw new Error(errorMsg);
    } finally {
      this.loading.set(false);
    }
  }

  async checkAvailability(request: CreateReservationRequest): Promise<ConflictCheck> {
    try {
      const response = await firstValueFrom(
        this.http.post<ConflictCheck>(`${this.apiUrl}/check-availability`, request)
      );
      return response;
    } catch (err: any) {
      const errorMsg = err.error?.message || err.error || 'Failed to check availability';
      throw new Error(errorMsg);
    }
  }

  async loadMyReservations(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      const response = await firstValueFrom(
        this.http.get<Reservation[]>(`${this.apiUrl}/my-reservations`)
      );
      this.myReservations.set(response);
    } catch (err: any) {
      const errorMsg = err.error?.message || err.error || 'Failed to load reservations';
      this.error.set(errorMsg);
      this.myReservations.set([]);
    } finally {
      this.loading.set(false);
    }
  }

  async cancelReservation(id: number): Promise<void> {
    this.loading.set(true);
    this.error.set(null);

    try {
      await firstValueFrom(
        this.http.delete(`${this.apiUrl}/${id}`)
      );
      await this.loadMyReservations();
    } catch (err: any) {
      const errorMsg = err.error?.message || err.error || 'Failed to cancel reservation';
      this.error.set(errorMsg);
      throw new Error(errorMsg);
    } finally {
      this.loading.set(false);
    }
  }

  hasActiveReservation(objectId: number): boolean {
    return this.myReservations().some(
      r => r.reservableObjectId === objectId && r.status === 'Active'
    );
  }
}
