import {GradePayload} from 'common/uonet/api/grades/GradePayload';
import {useStudent} from '../auth/StudentContext';
import {GetGradesByPupilQuery} from 'common/uonet/api/grades/GetGradesByPupilQuery';
import {Realm} from '@realm/react';
import {Student} from '../auth/Student.schema';
import {makeApiClient} from 'common/uonet/api/apiClient';
import {Grade} from './Grade.schema';
import {UpdateMode} from 'realm';
import {useSyncedResource} from 'common/data/useSyncedResource';
import {useRealm} from 'common/data/AppRealmContext';
import {getIdentities} from '../auth/clientIdentityStore';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {requestSigner} from 'common/uonet/signing/requestSigner';

const fetchGrades = async (student: Student) => {
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

  const request: GetGradesByPupilQuery = {
    unitId: student.unit.id,
    pupilId: student.pupil.id,
    pageSize: 500,
    lastId: (Math.pow(2, 32) / 2) * -1,
    lastSyncDate: new Date(0),
    periodId: student.periods[0].id,
  };

  const response = await apiClient.get<GradePayload[]>(
    GetGradesByPupilQuery.API_ENDPOINT,
    request,
  );

  return response.envelope;
};

const persistGrades = async (
  realm: Realm,
  student: Student,
  grades: GradePayload[],
) => {
  realm.write(() => {
    for (const grade of grades) {
      realm.create(
        'Grade',
        {
          id: grade.id,
          studentId: student.id,
          creatorName: grade.creator.name,
          pupilId: grade.pupilId,
          contentRaw: grade.contentRaw,
          content: grade.content,
          dateCreated: new Date(grade.dateCreated.timestamp),
          dateModify: grade.dateModify
            ? new Date(grade.dateModify.timestamp)
            : undefined,
          value: grade.value,
          column: {
            id: grade.column.id,
            key: grade.column.key,
            periodId: grade.column.periodId,
            name: grade.column.name,
            code: grade.column.code,
            group: grade.column.group,
            number: grade.column.number,
            color: grade.column.color,
            weight: grade.column.weight,
            subject: {
              id: grade.column.subject.id,
              key: grade.column.subject.key,
              name: grade.column.subject.name,
              kod: grade.column.subject.kod,
              position: grade.column.subject.position,
            },
          },
        },
        UpdateMode.Modified,
      );
    }
  });
};

export const useGrades = () => {
  const {activeStudent} = useStudent();
  const realm = useRealm();

  const {data} = useSyncedResource(
    Grade,
    [activeStudent.id, activeStudent.pupil.id],
    15,
    async () => await fetchGrades(activeStudent),
    async grades => await persistGrades(realm, activeStudent, grades),
  );

  return data.filtered(
    'studentId = $0',
    activeStudent?.id,
  ) as unknown as Realm.Results<Grade>;
};
