import {GradePayload} from 'common/uonet/api/grades/GradePayload';

export interface SubjectGrades {
  subjectName: string;
  subjectId: number;
  grades: GradePayload[];
}
