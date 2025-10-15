import { Injectable, computed, effect, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { AccessRequest, CreateAccessRequestRequest, RespondToAccessRequestRequest } from '../models/access-request';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AccessRequestService {
  private readonly http = inject(HttpClient);
  private readonly authService = inject(AuthService);
  private readonly apiUrl = 'http://localhost:5074';

  readonly myRequests = signal<AccessRequest[]>([]);
  readonly requestsForMyObjects = signal<AccessRequest[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly pendingRequestsCount = computed(() => 
    this.requestsForMyObjects().filter(r => r.status === 'Pending').length
  );

  readonly myPendingRequests = computed(() =>
    this.myRequests().filter(r => r.status === 'Pending')
  );

  readonly myRespondedRequests = computed(() =>
    this.myRequests().filter(r => r.status !== 'Pending')
  );

  constructor() {
    // Auto-load requests when user authenticates
    effect(() => {
      if (this.authService.isAuthenticated()) {
        this.loadMyRequests();
        this.loadRequestsForMyObjects();
      }
    });
  }

  async createRequest(request: CreateAccessRequestRequest): Promise<AccessRequest> {
    this.loading.set(true);
    this.error.set(null);
    
    try {
      const result = await firstValueFrom(
        this.http.post<AccessRequest>(`${this.apiUrl}/api/access-requests`, request)
      );
      
      await this.loadMyRequests();
      return result;
    } catch (err: any) {
      const errorMessage = err.error?.message || 'Failed to create access request';
      this.error.set(errorMessage);
      throw new Error(errorMessage);
    } finally {
      this.loading.set(false);
    }
  }

  async loadMyRequests(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    
    try {
      const requests = await firstValueFrom(
        this.http.get<AccessRequest[]>(`${this.apiUrl}/api/access-requests/my-requests`)
      );
      this.myRequests.set(requests);
    } catch (err: any) {
      console.error('Failed to load my requests:', err);
      this.error.set('Failed to load your requests');
      this.myRequests.set([]);
    } finally {
      this.loading.set(false);
    }
  }

  async loadRequestsForMyObjects(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    
    try {
      const requests = await firstValueFrom(
        this.http.get<AccessRequest[]>(`${this.apiUrl}/api/access-requests/my-owned-objects-requests`)
      );
      this.requestsForMyObjects.set(requests);
    } catch (err: any) {
      console.error('Failed to load requests for my objects:', err);
      this.error.set('Failed to load requests for your objects');
      this.requestsForMyObjects.set([]);
    } finally {
      this.loading.set(false);
    }
  }

  async respondToRequest(id: number, response: RespondToAccessRequestRequest): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    
    try {
      await firstValueFrom(
        this.http.post(`${this.apiUrl}/api/access-requests/${id}/respond`, response)
      );
      
      await this.loadRequestsForMyObjects();
    } catch (err: any) {
      const errorMessage = err.error?.message || 'Failed to respond to request';
      this.error.set(errorMessage);
      throw new Error(errorMessage);
    } finally {
      this.loading.set(false);
    }
  }
}
