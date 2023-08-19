import axios from 'axios';
import {
  ClientIdentity,
  RequestSigner,
} from 'common/uonet/signing/requestSigner';
import {
  parsePascalCase,
  stringifyPascalCase,
} from 'common/uonet/api/jsonPayloadUtility';

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
  post<T, R extends {} = {}>(url: string, request: R): Promise<ApiResponse<T>>;
  get<T, R extends {} = {}>(url: string, query?: R): Promise<ApiResponse<T>>;
}

const toQueryString = <T extends {}>(query: T) => {
  const keys = Object.keys(query) as (keyof T)[];

  if (keys.length === 0) {
    return '';
  }

  const pairs = keys.map(k => `${String(k)}=${query[k]}`);

  return '?' + pairs.join('&');
};

export const makeApiClient = (
  requestSigner: RequestSigner,
  clientIdentity: ClientIdentity,
  apiInstanceUrl: string,
  accountContext?: string,
): ApiClient => {
  const post = async <T, R extends {} = {}>(
    url: string,
    request: R,
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

    const {data} = await axios.post<ApiResponse<T>>(fullUrl, json, {
      headers,
      transformResponse: parsePascalCase,
    });

    return data;
  };

  const get = async <T, R extends {} = {}>(
    url: string,
    query?: R,
  ): Promise<ApiResponse<T>> => {
    let fullUrl = apiInstanceUrl + '/' + url;
    if (query !== undefined) {
      fullUrl += toQueryString(query);
    }

    const headers = requestSigner.createSignedHeaders(
      fullUrl,
      clientIdentity,
      undefined,
      accountContext,
    );

    const {data} = await axios.get<ApiResponse<T>>(fullUrl, {
      headers,
      transformResponse: parsePascalCase,
    });

    return data;
  };

  return {
    post,
    get,
  };
};
