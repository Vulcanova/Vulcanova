import {Realm} from '@realm/react';
import {Student} from './Student.schema';
import React, {createContext, useCallback, useContext} from 'react';
import {useQuery, useRealm} from 'common/data/AppRealmContext';
import {CompositeNavigationProp, useNavigation} from '@react-navigation/native';
import {StackNavigationProp} from '@react-navigation/stack';
import {StackParamList} from '../../App';
import {BottomTabNavigationProp} from '@react-navigation/bottom-tabs';
import {TabParamList} from '../TabsScreen';

export interface StudentContextType {
  activeStudent: Student;
  changeActiveStudent(studentId: Realm.BSON.ObjectId): void;
}

export const StudentContext = createContext<StudentContextType | null>(null);

interface Props {
  children: React.ReactNode;
}

type ContextNavigationProp = CompositeNavigationProp<
  StackNavigationProp<StackParamList, 'Tabs'>,
  BottomTabNavigationProp<TabParamList>
>;

export const StudentProvider = ({children}: Props) => {
  const realm = useRealm();
  const students = useQuery(Student);
  const navigation = useNavigation<ContextNavigationProp>();
  const activeStudent = students.filtered('isActive = true')[0] ?? null;

  const changeActiveStudent = useCallback(
    (studentId: Realm.BSON.ObjectId) => {
      realm.write(() => {
        for (const student of students) {
          student.isActive = student.id === studentId;
        }
      });
    },
    [realm, students],
  );

  if (activeStudent === null) {
    navigation.replace('Intro');
  }

  const value = {activeStudent, changeActiveStudent};

  return (
    <StudentContext.Provider value={value}>{children}</StudentContext.Provider>
  );
};

export const useStudent = () =>
  useContext(StudentContext) as StudentContextType;
