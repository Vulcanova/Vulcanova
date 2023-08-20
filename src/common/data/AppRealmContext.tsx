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
import {Column, Grade} from '../../features/grades/Grade.schema';
import {Subject} from '../../features/shared/Subject.schema';

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
    Grade,
    Column,
    Subject,
  ],
  deleteRealmIfMigrationNeeded: true,
});

export const useRealm = AppRealmContext.useRealm;
export const useQuery = AppRealmContext.useQuery;
