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

const fetchFinalGrades = async (student: Student, periodId: number) => {
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
    periodId: periodId,
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

export const useFinalGrades = (periodId: number) => {
  const {activeStudent} = useStudent();
  const realm = useRealm();

  const {data, isLoading, refetch} = useSyncedResource(
    FinalGradesEntry,
    ['finalGrades', activeStudent.id, activeStudent.pupil.id, periodId],
    60,
    async () => await fetchFinalGrades(activeStudent, periodId),
    async grades => await persistFinalGrades(realm, activeStudent, grades),
  );

  return {
    data: data.filtered(
      'studentId = $0 && periodId = $1',
      activeStudent?.id,
      periodId,
    ) as unknown as Realm.Results<FinalGradesEntry>,
    isLoading,
    refetch,
  };
};
