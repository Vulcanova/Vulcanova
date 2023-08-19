import {clientConstants} from 'common/uonet/clientConstants';
import 'fast-text-encoding';
import quickCrypto from 'react-native-quick-crypto';
import dateFormat from 'dateformat';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {getSignatureHeaders} from 'common/uonet/signing/valuesSigner';

export interface ClientIdentity {
  certificate: string;
  privateKey: string;
  firebaseToken: string;
}

export interface SignedApiPayload<T> {
  appName: string;
  appVersion: string;
  certificateId: string;
  envelope: T;
  firebaseToken: string;
  API: number;
  requestId: string;
  timestamp: number;
  timestampFormatted: string;
}

export interface RequestSigner {
  signPayload: <T>(
    payload: T,
    identity: ClientIdentity,
  ) => Promise<SignedApiPayload<T>>;
  createSignedHeaders: (
    fullUrl: string,
    identity: ClientIdentity,
    body?: string,
    accountContext?: string,
  ) => Record<string, string>;
}

const signPayload = async <T>(
  payload: T,
  identity: ClientIdentity,
): Promise<SignedApiPayload<T>> => {
  const now = new Date();

  return {
    appName: clientConstants.appName,
    appVersion: clientConstants.appVersion,
    certificateId: getCertThumbprint(identity.certificate),
    firebaseToken: identity.firebaseToken,
    API: 1,
    requestId: quickCrypto.randomUUID(),
    timestamp: Date.now(),
    timestampFormatted: dateFormat(now, 'yyyy-M-d HH:mm:ss'),
    envelope: payload,
  };
};

const createSignedHeaders = (
  fullUrl: string,
  identity: ClientIdentity,
  body?: string,
) => ({
  ...getSignatureHeaders(
    getCertThumbprint(identity.certificate),
    identity.privateKey,
    fullUrl,
    body,
  ),
  ['User-Agent']: clientConstants.userAgent,
  ['vOS']: clientConstants.appOs,
  ['vDeviceModel']: clientConstants.deviceModel,
  ['vAPI']: '1',
  ['Content-Type']: 'application/json',
});

export const requestSigner: RequestSigner = {
  signPayload,
  createSignedHeaders,
};
