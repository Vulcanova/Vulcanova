import {ClientIdentity} from 'common/uonet/signing/requestSigner';
import {generateCertificate} from 'common/uonet/crypto/certificateGenerator';
import {fetchFirebaseToken} from 'common/uonet/firebase/firebaseTokenFetcher';

export const generateClientIdentity = async (): Promise<ClientIdentity> => {
  const {certificate, privateKey} = await generateCertificate();
  const firebaseToken = await fetchFirebaseToken();

  return {
    certificate,
    privateKey,
    firebaseToken,
  };
};
