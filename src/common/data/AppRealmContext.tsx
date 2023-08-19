import {createRealmContext} from '@realm/react';
import {
  ConstituentUnit,
  Login,
  Period,
  Pupil,
  SenderEntry,
  Student,
  Unit,
} from '../../features/auth/Student.schema';
import {Account} from '../../features/auth/Account.schema';

export const AppRealmContext = createRealmContext({
  schema: [
    Account,
    Student,
    Pupil,
    Period,
    Unit,
    ConstituentUnit,
    Login,
    SenderEntry,
  ],
  deleteRealmIfMigrationNeeded: true,
});

export const useRealm = AppRealmContext.useRealm;
export const useQuery = AppRealmContext.useQuery;
