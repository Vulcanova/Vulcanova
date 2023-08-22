import {Realm} from '@realm/react';
import {Subject} from '../../shared/Subject.schema';

export class FinalGradesEntry extends Realm.Object<FinalGradesEntry> {
  id!: string;
  studentId!: Realm.BSON.ObjectId;
  pupilId!: number;
  periodId!: number;
  subject!: Subject;
  predictedGrade?: string;
  finalGrade?: string;
  entry3?: string;
  dateModify!: Date;

  static schema: Realm.ObjectSchema = {
    name: 'FinalGradesEntry',
    primaryKey: 'id',
    properties: {
      id: 'string',
      studentId: 'objectId',
      pupilId: 'int',
      periodId: 'int',
      subject: 'Subject',
      predictedGrade: 'string?',
      finalGrade: 'string?',
      entry3: 'string?',
      dateModify: 'date',
    },
  };
}
