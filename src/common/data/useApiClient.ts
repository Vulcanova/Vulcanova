import {useStudent} from '../../features/auth/StudentContext';
import {useEffect, useState} from 'react';
import {requestSigner} from 'common/uonet/signing/requestSigner';
import {getIdentities} from '../../features/auth/clientIdentityStore';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {ApiClient, makeApiClient} from 'common/uonet/api/apiClient';

export const useApiClient = () => {
  const {activeStudent} = useStudent();
  const [apiClient, setApiClient] = useState<ApiClient | null>(null);
  if (activeStudent === null) {
    throw new Error(
      'useApiClient hook is meant to be used in authenticated context',
    );
  }

  useEffect(() => {
    const fetchIdentity = async () => {
      const identities = await getIdentities();
      const studentClientIdentity = identities.find(
        i =>
          getCertThumbprint(i.certificate) === activeStudent.identityThumbprint,
      );

      // TODO: Handle studentClientIdentity undefined
      const createdApiClient = makeApiClient(
        requestSigner,
        studentClientIdentity!,
        activeStudent.unit.restURL,
        activeStudent.context,
      );

      setApiClient(createdApiClient);
    };

    fetchIdentity();
  }, [activeStudent]);

  return apiClient;
};
