import {Subject} from '../common/Subject';
import {DateTimeInfo} from 'common/uonet/api/common/DateTimeInfo';

export interface GradesSummaryEntryPayload {
  id: number;
  pupilId: number;
  periodId: number;
  subject: Subject;
  entry_1: string;
  entry_2: string;
  entry_3: string;
  dateModify: DateTimeInfo;
}
