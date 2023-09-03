import {View} from 'react-native';
import GradesList from './GradesList';
import {useGrades} from '../useGrades';
import {usePeriod} from '../PeriodContext';
import SubjectGradesDetailsSheet, {
  SubjectGradesDetailsSheetHandle,
} from './subjectGradesDetails/SubjectGradesDetailsSheet';
import {useRef} from 'react';
import {SubjectGrades} from './types';

const GradesSummaryScreen = () => {
  const sheetRef = useRef<SubjectGradesDetailsSheetHandle>(null);
  const {activePeriodId} = usePeriod();
  const {data, refetch, isLoading} = useGrades(activePeriodId);

  const handleSubjectGradesPress = (subjectGrades: SubjectGrades) => {
    sheetRef.current?.present(subjectGrades);
  };

  return (
    <View>
      <GradesList
        grades={data}
        isRefreshing={isLoading}
        onRefresh={refetch}
        onSubjectGradesPress={handleSubjectGradesPress}
      />
      <SubjectGradesDetailsSheet ref={sheetRef} />
    </View>
  );
};

export default GradesSummaryScreen;
