import {useRealm} from 'common/data/AppRealmContext';
import {Student} from './Student.schema';
import {AccountPayload} from 'common/uonet/api/auth/AccountPayload';
import {StudentPayload} from 'common/uonet/api/auth/StudentPayload';

export const useAccountsManagement = () => {
  const realm = useRealm();

  const addAccount = (
    account: AccountPayload,
    students: StudentPayload[],
    identityThumbprint: string,
  ) => {
    realm.write(() => {
      let createdStudents: Student[] = [];
      let isFirst = true;
      for (const student of students) {
        const createdStudent = realm.create('Student', {
          ...student,
          identityThumbprint,
          isActive: isFirst,
          periods: undefined,
        });

        // @ts-ignore
        createdStudents.push(createdStudent);

        for (const period of student.periods) {
          // @ts-ignore
          createdStudent.periods.push({
            id: period.id,
            level: period.level,
            number: period.number,
            current: period.current,
            start: new Date(period.start.timestamp),
            end: new Date(period.end.timestamp),
          });
        }

        isFirst = false;
      }

      realm.create(
        'Account',
        {
          ...account,
          students: createdStudents,
          identityThumbprint,
        },
        true,
      );

      const existingStudents = realm
        .objects(Student)
        .filtered('id != $0', createdStudents[0].id);

      for (const student of existingStudents) {
        student.isActive = false;
      }
    });
  };

  return {addAccount};
};
