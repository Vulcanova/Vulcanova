import {Realm} from '@realm/react';
import {makeApiClient} from 'common/uonet/api/apiClient';
import {UpdateMode} from 'realm';
import {useSyncedResource} from 'common/data/useSyncedResource';
import {useRealm} from 'common/data/AppRealmContext';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {requestSigner} from 'common/uonet/signing/requestSigner';
import {GetGradesSummaryByPupilQuery} from 'common/uonet/api/grades/GetGradesSummaryByPupilQuery';
import {Student} from '../../auth/Student.schema';
import {getIdentities} from '../../auth/clientIdentityStore';
import {GradesSummaryEntryPayload} from 'common/uonet/api/grades/GradesSummaryEntryPayload';
import {FinalGradesEntry} from './FinalGradesEntry.schema';
import {useStudent} from '../../auth/StudentContext';

const fetchFinalGrades = async (student: Student) => {
  const identities = await getIdentities();
  const studentClientIdentity = identities.find(
    i => getCertThumbprint(i.certificate) === student.identityThumbprint,
  );

  const apiClient = makeApiClient(
    requestSigner,
    studentClientIdentity!,
    student.unit.restURL,
    student.context,
  );

  const request: GetGradesSummaryByPupilQuery = {
    unitId: student.unit.id,
    pupilId: student.pupil.id,
    pageSize: 500,
    lastId: (Math.pow(2, 32) / 2) * -1,
    periodId: student.periods[0].id,
  };

  const response = await apiClient.get<GradesSummaryEntryPayload[]>(
    GetGradesSummaryByPupilQuery.API_ENDPOINT,
    request,
  );

  return response.envelope;
};

const persistFinalGrades = async (
  realm: Realm,
  student: Student,
  grades: GradesSummaryEntryPayload[],
) => {
  realm.write(() => {
    for (const grade of grades) {
      realm.create(
        'FinalGradesEntry',
        {
          id: `${grade.periodId}_${grade.id}`,
          studentId: student.id,
          pupilId: grade.pupilId,
          periodId: grade.periodId,
          predictedGrade: grade.entry_1,
          finalGrade: grade.entry_2,
          entry3: grade.entry_3,
          dateModify: new Date(grade.dateModify.timestamp),
          subject: {
            id: grade.subject.id,
            key: grade.subject.key,
            name: grade.subject.name,
            kod: grade.subject.kod,
            position: grade.subject.position,
          },
        },
        UpdateMode.Modified,
      );
    }
  });
};

export const useFinalGrades = () => {
  const {activeStudent} = useStudent();
  const realm = useRealm();

  console.log('use!');

  const {data} = useSyncedResource(
    FinalGradesEntry,
    ['finalGrades', activeStudent.id, activeStudent.pupil.id],
    60,
    async () => await fetchFinalGrades(activeStudent),
    async grades => await persistFinalGrades(realm, activeStudent, grades),
  );

  return data.filtered(
    'studentId = $0',
    activeStudent?.id,
  ) as unknown as Realm.Results<FinalGradesEntry>;
};
