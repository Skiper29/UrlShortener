export interface LoginRequest {
  login: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  userName: string;
  email: string;
  role: string;
}

export interface AuthUser {
  userName: string;
  email: string;
  role: 'Admin' | 'User';
}