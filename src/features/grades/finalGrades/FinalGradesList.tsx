import {Realm} from '@realm/react';
import {FinalGradesEntry} from './FinalGradesEntry.schema';
import {FlatList, RefreshControl} from 'react-native';
import FinalGradesListSubject from './FinalGradesListSubject';

export interface FinalGradesListProps {
  finalGrades: Realm.Results<FinalGradesEntry>;
  onRefresh(): void;
  isRefreshing: boolean;
}

const FinalGradesList = ({
  finalGrades,
  onRefresh,
  isRefreshing,
}: FinalGradesListProps) => {
  return (
    <FlatList
      refreshControl={
        <RefreshControl refreshing={isRefreshing} onRefresh={onRefresh} />
      }
      data={finalGrades.sorted('subject.name')}
      keyExtractor={item => item.id}
      renderItem={({item}) => (
        <FinalGradesListSubject finalGradesEntry={item} />
      )}
    />
  );
};

export default FinalGradesList;
