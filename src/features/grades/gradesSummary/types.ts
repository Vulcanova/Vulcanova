import {Grade} from '../Grade.schema';

export interface SubjectGrades {
  subjectName: string;
  subjectId: number;
  grades: Grade[];
}
