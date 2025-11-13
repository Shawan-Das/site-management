export interface Satcom {
  id?: number;
  company: string;
  category: 'Online' | 'Virtual' | 'Hybrid' | 'Physical';
  type: 'EGM' | 'AGM' | 'AGM & EGM' | 'UHM';
  date: string;
  time: string;
  db_port: string;
  ui_port: string;
  url: string;
  ip: string;
  status: boolean;
}

export interface SatcomResponse {
  statusCode: number;
  serviceMessage: string;
  isSuccess: boolean;
  payload?: Satcom[];
  ts?: string;
}

