import {generateClientIdentity} from './clientIdentityGenerator';
import {getDeviceName} from 'react-native-device-info';
import {
  API_ENDPOINT,
  RegisterClientRequest,
} from 'common/uonet/api/auth/RegisterClientRequest';
import {clientConstants} from 'common/uonet/clientConstants';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import quickCrypto from 'react-native-quick-crypto';
import {makeApiClient} from 'common/uonet/api/apiClient';
import {requestSigner} from 'common/uonet/signing/requestSigner';

export const authenticate = async (
  token: string,
  pin: string,
  instanceUrl: string,
) => {
  const identity = await generateClientIdentity();
  const device = `Vulcanova React Native — ${await getDeviceName()}`;

  const certInfo = identity.certificate
    .replace('-----BEGIN CERTIFICATE-----', '')
    .replace('-----END CERTIFICATE-----', '')
    .replaceAll('\r\n', '');

  const request: RegisterClientRequest = {
    OS: clientConstants.appOs,
    deviceModel: device,
    certificate: certInfo,
    certificateType: 'X509',
    certificateThumbprint: getCertThumbprint(identity.certificate),
    PIN: pin,
    securityToken: token,
    selfIdentifier: quickCrypto.randomUUID(),
  };

  const client = makeApiClient(requestSigner, identity, instanceUrl);

  await client.post(API_ENDPOINT, request);
};
