export interface UrlResponse {
  id: number;
  originalUrl: string;
  shortCode: string;
  shortUrl: string;
  createdBy: string;
  createdAt: string;
}

export interface UrlDetailResponse {
  id: number;
  originalUrl: string;
  shortCode: string;
  shortUrl: string;
  createdById: string;
  createdByUserName: string;
  createdByEmail: string;
  createdAt: string;
}

export interface CreateUrlRequest {
  originalUrl: string;
}