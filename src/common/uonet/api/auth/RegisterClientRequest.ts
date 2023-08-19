import {ApiRequest} from 'common/uonet/api/types';
import {RegisterClientResponse} from 'common/uonet/api/auth/RegisterClientResponse';

export interface RegisterClientRequest
  extends ApiRequest<RegisterClientResponse> {
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
