import {Realm} from '@realm/react';
import {Student} from './Student.schema';

export class Account extends Realm.Object<Account> {
  id!: Realm.BSON.UUID;
  identityThumbprint!: string;
  restURL!: string;
  loginId!: number;
  userLogin!: string;
  userName!: string;
  students!: Realm.List<Student>;

  static schema: Realm.ObjectSchema = {
    name: 'Account',
    properties: {
      id: {type: 'objectId', default: () => new Realm.BSON.ObjectId()},
      identityThumbprint: 'string',
      restURL: 'string',
      loginId: 'int',
      userLogin: 'string',
      userName: 'string',
      students: {type: 'list', objectType: 'Student'},
    },
    primaryKey: 'id',
  };
}
