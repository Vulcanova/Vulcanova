import {Realm} from '@realm/react';
import {FinalGradesEntry} from './FinalGradesEntry.schema';
import {FlatList} from 'react-native';
import FinalGradesListSubject from './FinalGradesListSubject';

export interface FinalGradesListProps {
  finalGrades: Realm.Results<FinalGradesEntry>;
}

const FinalGradesList = ({finalGrades}: FinalGradesListProps) => {
  return (
    <FlatList
      data={finalGrades}
      keyExtractor={item => item.id}
      renderItem={({item}) => (
        <FinalGradesListSubject finalGradesEntry={item} />
      )}
    />
  );
};

export default FinalGradesList;
