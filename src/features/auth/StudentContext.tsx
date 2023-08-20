import {Realm} from '@realm/react';
import {Student} from './Student.schema';
import React, {createContext, useCallback, useContext} from 'react';
import {useQuery, useRealm} from 'common/data/AppRealmContext';

export interface StudentContextType {
  activeStudent: Student | null;
  changeActiveStudent(studentId: Realm.BSON.UUID): void;
}

export const StudentContext = createContext<StudentContextType | null>(null);

interface Props {
  children: React.ReactNode;
}

export const StudentProvider = ({children}: Props) => {
  const realm = useRealm();
  const students = useQuery(Student);
  const activeStudent = students.filtered('isActive = true')[0] ?? null;

  const changeActiveStudent = useCallback(
    (studentId: Realm.BSON.UUID) => {
      realm.write(() => {
        for (const student of students) {
          student.isActive = student.id === studentId;
        }
      });
    },
    [realm, students],
  );

  const value = {activeStudent, changeActiveStudent};

  return (
    <StudentContext.Provider value={value}>{children}</StudentContext.Provider>
  );
};

export const useStudent = () =>
  useContext(StudentContext) as StudentContextType;
