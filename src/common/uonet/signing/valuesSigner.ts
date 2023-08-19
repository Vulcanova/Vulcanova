import {QuickCrypto} from 'react-native-quick-crypto/src/QuickCrypto';
import rs from 'jsrsasign';

const getEncodedPath = (path: string): string => {
  const rx = path.match(/(api\/mobile\/.+)/);
  return rx ? encodeURIComponent(rx[1]).toLowerCase() : '';
};

const getDigest = (body: string): string => {
  const sha = QuickCrypto.createHash('sha256');
  sha.update(body, 'utf8');
  const data = sha.digest();
  return data.toString('base64');
};

const buildSignatureHeaderValue = (
  headers: Record<string, string>,
  fingerprint: string,
  privateKey: string,
): string => {
  return (
    `keyId="${fingerprint}",headers="${Object.keys(headers).join(
      ' ',
    )}",algorithm="SHA256withECDSA",` +
    `signature=Base64(SHA256withECDSA(${getHeadersSignature(
      Object.values(headers).join(''),
      privateKey,
    )}))`
  );
};

export const getSignatureHeaders = (
  fingerprint: string,
  privateKey: string,
  requestUrl: string,
  body?: string,
  timestamp: Date = new Date(),
): Record<string, string> => {
  const formattedTimestamp = timestamp.toUTCString();

  const headers: Record<string, string> = {
    vCanonicalUrl: getEncodedPath(requestUrl),
    vDate: formattedTimestamp,
  };

  if (body) {
    const digest = getDigest(body);
    headers.Digest = `SHA-256=${digest}`;
  }

  headers.Signature = buildSignatureHeaderValue(
    headers,
    fingerprint,
    privateKey,
  );

  return headers;
};

const getHeadersSignature = (values: string, privateKey: string): string => {
  const sig = new rs.KJUR.crypto.Signature({alg: 'SHA256withECDSA'});
  sig.init(privateKey);
  sig.updateString(values);
  const sigValueHex = sig.sign();
  return rs.hextob64(sigValueHex);
};
