import 'fast-text-encoding';
import rs from 'jsrsasign';

export const getCertThumbprint = (pemString: string) => {
  const hex = rs.pemtohex(pemString);
  return rs.KJUR.crypto.Util.hashHex(hex, 'sha1');
};
