import {Realm} from '@realm/react';

export class Subject extends Realm.Object<Subject> {
  id!: number;
  key!: string;
  name!: string;
  kod!: string;
  position!: number;

  static schema: Realm.ObjectSchema = {
    name: 'Subject',
    embedded: true,
    properties: {
      id: 'int',
      key: 'string',
      name: 'string',
      kod: 'string',
      position: 'int',
    },
  };
}
