import {Teacher} from 'common/uonet/api/common/Teacher';
import {Subject} from 'common/uonet/api/common/Subject';
import {DateTimeInfo} from 'common/uonet/api/common/DateTimeInfo';

export interface GradePayload {
  id: number;
  key: string;
  pupilId: number;
  contentRaw: string;
  content: string;
  comment: string;
  value: number | null;
  numerator: any;
  denominator: any;
  dateCreated: DateTimeInfo;
  dateModify?: DateTimeInfo;
  creator: Teacher;
  modifier?: Teacher;
  column: Column;
}

export interface Column {
  id: number;
  key: string;
  periodId: number;
  name: string;
  code: string;
  group: string;
  number: number;
  color: number;
  weight: number;
  subject: Subject;
  category: Category;
}

export interface Category {
  id: number;
  name: string;
  code: string;
}
