export interface RegisterClientRequest {
  OS: string;
  deviceModel: string;
  certificate: string;
  certificateType: string;
  certificateThumbprint: string;
  PIN: string;
  securityToken: string;
  selfIdentifier: string;
}

export const API_ENDPOINT = 'api/mobile/register/new';
