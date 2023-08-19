import axios from 'axios';
import {ApiQuery, ApiRequest} from 'common/uonet/api/types';
import {
  ClientIdentity,
  RequestSigner,
} from 'common/uonet/signing/requestSigner';
import {stringifyPascalCase} from 'common/uonet/api/jsonPayloadUtility';

export interface ApiResponse<T> {
  envelopeType: string;
  envelope: T;
  status: {code: number; message: string};
  requestId: string;
  timestamp: number;
  timestampFormatted: string;
  inResponseTo: object;
}

export interface ApiClient {
  post<T>(url: string, request: ApiRequest<T>): Promise<ApiResponse<T>>;
  get<T>(url: string, query: ApiQuery<T>): Promise<ApiResponse<T>>;
}

const toQueryString = <T>(query: ApiQuery<T>) => {
  const keys = Object.keys(query) as (keyof typeof query)[];

  if (keys.length === 0) {
    return '';
  }

  const pairs = keys.map(k => `${k}=${query[k]}`);

  return '?' + pairs.join('&');
};

export const makeApiClient = (
  requestSigner: RequestSigner,
  clientIdentity: ClientIdentity,
  apiInstanceUrl: string,
  accountContext?: string,
): ApiClient => {
  const post = async <T>(
    url: string,
    request: ApiRequest<T>,
  ): Promise<ApiResponse<T>> => {
    const fullUrl = apiInstanceUrl + '/' + url;
    const signed = await requestSigner.signPayload(request, clientIdentity);
    const json = stringifyPascalCase(signed);

    const headers = requestSigner.createSignedHeaders(
      fullUrl,
      clientIdentity,
      json,
      accountContext,
    );

    const {data} = await axios.post(fullUrl, json, {
      headers,
    });

    return data as ApiResponse<T>;
  };

  const get = async <T>(
    url: string,
    request: ApiQuery<T>,
  ): Promise<ApiResponse<T>> => {
    const fullUrl = apiInstanceUrl + '/' + url + toQueryString(request);

    const headers = requestSigner.createSignedHeaders(
      fullUrl,
      clientIdentity,
      undefined,
      accountContext,
    );

    const {data} = await axios.get(fullUrl, {
      headers,
    });

    return data as ApiResponse<T>;
  };

  return {
    post,
    get,
  };
};
