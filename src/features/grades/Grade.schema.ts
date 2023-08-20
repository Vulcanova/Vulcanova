import {Realm} from '@realm/react';
import {Subject} from '../shared/Subject.schema';

export class Grade extends Realm.Object<Grade> {
  id!: number;
  studentId!: Realm.BSON.ObjectId;
  creatorName!: string;
  pupilId!: number;
  contentRaw!: string;
  content!: string;
  comment?: string;
  dateCreated!: Date;
  dateModify?: Date;
  value?: number;
  column!: Column;

  static schema: Realm.ObjectSchema = {
    name: 'Grade',
    properties: {
      id: 'int',
      studentId: 'objectId',
      creatorName: 'string',
      pupilId: 'int',
      contentRaw: 'string',
      content: 'string',
      comment: 'string?',
      dateCreated: 'date',
      dateModify: 'date?',
      value: 'int?',
      column: 'Column',
    },
    primaryKey: 'id',
  };
}

export class Column extends Realm.Object<Column> {
  id!: number;
  key!: string;
  periodId!: number;
  name!: string;
  code!: string;
  group!: string;
  number!: number;
  color!: number;
  weight!: number;
  subject!: Subject;

  static schema: Realm.ObjectSchema = {
    name: 'Column',
    embedded: true,
    properties: {
      id: 'int',
      key: 'string',
      periodId: 'int',
      name: 'string',
      code: 'string',
      group: 'string',
      number: 'int',
      color: 'int',
      weight: 'int',
      subject: 'Subject',
    },
  };
}
