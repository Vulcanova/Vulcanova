import {ApiClientType} from 'common/uonet/api/apiClient';

export interface UonetContextType {
  useApiClient(): ApiClientType;
}
