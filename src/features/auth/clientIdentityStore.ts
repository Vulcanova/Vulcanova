import EncryptedStorage from 'react-native-encrypted-storage';
import {ClientIdentity} from 'common/uonet/signing/requestSigner';

export const getIdentities = async (): Promise<ClientIdentity[]> => {
  try {
    const identities = await EncryptedStorage.getItem('client_identities');

    if (identities === null) {
      return [];
    }

    return JSON.parse(identities);
  } catch {
    return [];
  }
};

export const storeIdentity = async (identity: ClientIdentity) => {
  const identities = await getIdentities();
  identities.push(identity);

  await EncryptedStorage.setItem(
    'client_identities',
    JSON.stringify(identities),
  );
};
