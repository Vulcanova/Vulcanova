import {useQuery} from 'common/data/AppRealmContext';
import {useQuery as useRemoteQuery} from 'react-query';
import {QueryKey} from 'react-query/types/core/types';

type SchemaType = Parameters<typeof useQuery>[0];

export const useSyncedResource = <
  TSchema extends SchemaType,
  TRemote,
  TQueryKey extends QueryKey = QueryKey,
>(
  type: TSchema,
  queryKey: TQueryKey,
  offlineCacheLifespanMinutes: number,
  fetchData: () => Promise<TRemote>,
  persistData: (data: TRemote) => Promise<any>,
) => {
  const localData = useQuery(type) as unknown as Realm.Results<TSchema>;

  const {isLoading} = useRemoteQuery(queryKey, fetchData, {
    staleTime: offlineCacheLifespanMinutes * (60 * 1000),
    cacheTime: (offlineCacheLifespanMinutes + 1) * (60 * 1000),
    retry: false,
    refetchOnWindowFocus: false,
    onSuccess: async data => {
      await persistData(data);
    },
  });

  return {data: localData, isSyncing: isLoading};
};
