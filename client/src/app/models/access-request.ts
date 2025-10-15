export interface AccessRequest {
  id: number;
  reservableObjectId: number;
  reservableObjectName: string;
  requesterUsername: string;
  requestedStartTime: string;
  requestedEndTime: string;
  status: AccessRequestStatus;
  message?: string;
  responseMessage?: string;
  createdAt: string;
  respondedAt?: string;
}

export type AccessRequestStatus = 'Pending' | 'Approved' | 'Denied';

export interface CreateAccessRequestRequest {
  reservableObjectId: number;
  requestedStartTime: string;
  requestedEndTime: string;
  message?: string;
}

export interface RespondToAccessRequestRequest {
  status: 'Approved' | 'Denied';
  responseMessage?: string;
}
