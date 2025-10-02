export interface ReservableObject {
  id: number;
  name: string;
  type: string;
  isAvailable: boolean;
  hasTimeRestrictions: boolean;
  availableFrom?: string;
  availableUntil?: string;
  daysOfWeek?: string;
  positionX?: number;
  positionY?: number;
}

export interface ReservableObjectListResponse {
  totalCount: number;
  objects: ReservableObject[];
}

export const ObjectTypes = {
  Desk: 'Desk',
  ParkingSpace: 'ParkingSpace',
  All: 'All'
} as const;

export type ObjectType = typeof ObjectTypes[keyof typeof ObjectTypes];
