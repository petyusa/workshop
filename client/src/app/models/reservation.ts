export interface Reservation {
  id: number;
  reservableObjectId: number;
  objectName: string;
  objectType: string;
  locationId: number;
  locationName: string;
  username: string;
  startTime: string;
  endTime: string;
  status: 'Active' | 'Cancelled' | 'Completed';
  createdAt: string;
}

export interface CreateReservationRequest {
  reservableObjectId: number;
  startTime: string;
  endTime: string;
}

export interface ConflictCheck {
  hasConflict: boolean;
  message?: string;
  conflicts?: ConflictingReservation[];
}

export interface ConflictingReservation {
  id: number;
  startTime: string;
  endTime: string;
}
