import * as rs from 'jsrsasign';

export const generateCertificate = () => {
  const kp = rs.KEYUTIL.generateKeypair('EC', 'secp256r1');
  const prv = kp.prvKeyObj;
  const pub = kp.pubKeyObj;
  const prvpem = rs.KEYUTIL.getPEM(prv, 'PKCS8PRV');

  const x = new rs.KJUR.asn1.x509.Certificate({
    version: 3,
    serial: {int: 4},
    issuer: {str: '/CN=UserCA'},
    notbefore: '201231235959Z',
    notafter: '221231235959Z',
    subject: {str: '/CN=User1'},
    sbjpubkey: pub,
    ext: [],
    sigalg: 'SHA256withECDSA',
    cakey: prv,
  });

  return {certificate: x.getPEM(), privateKey: prvpem};
};
