import {useFinalGrades} from './useFinalGrades';
import FinalGradesList from './FinalGradesList';
import {usePeriod} from '../PeriodContext';

const FinalGradesScreen = () => {
  const {activePeriodId} = usePeriod();
  const {data, isLoading, refetch} = useFinalGrades(activePeriodId);

  return (
    <FinalGradesList
      finalGrades={data}
      onRefresh={refetch}
      isRefreshing={isLoading}
    />
  );
};

export default FinalGradesScreen;
