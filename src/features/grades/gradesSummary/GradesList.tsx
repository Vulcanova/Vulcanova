import {FlatList} from 'react-native';
import {useMemo} from 'react';
import {SubjectGrades} from './types';
import GradesListSubject from './GradesListSubject';
import {Grade} from '../Grade.schema';
import {Realm} from '@realm/react';

export interface GradesListProps {
  grades: Realm.Results<Grade>;
}

const GradesList = ({grades}: GradesListProps) => {
  const subjectsWithGrades = useMemo(() => {
    const groups = grades.reduce((prev, curr) => {
      const existingGroup = prev.find(
        s => s.subjectId === curr.column.subject.id,
      );
      if (existingGroup) {
        existingGroup.grades.push(curr);
        return prev;
      } else {
        const group: SubjectGrades = {
          subjectName: curr.column.subject.name,
          subjectId: curr.column.subject.id,
          grades: [curr],
        };
        return [...prev, group];
      }
    }, [] as SubjectGrades[]);

    return groups.sort((a, z) => {
      if (a.subjectName < z.subjectName) {
        return -1;
      }
      if (a.subjectName > z.subjectName) {
        return 1;
      }
      return 0;
    });
  }, [grades]);

  return (
    <FlatList
      data={subjectsWithGrades}
      keyExtractor={item => item.subjectId.toString()}
      renderItem={({item}) => <GradesListSubject subject={item} />}
    />
  );
};

export default GradesList;
