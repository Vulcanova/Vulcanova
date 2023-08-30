import {View} from 'react-native';
import GradesList from './GradesList';
import {useGrades} from '../useGrades';
import {usePeriod} from '../PeriodContext';

const GradesSummaryScreen = () => {
  const {activePeriodId} = usePeriod();
  const {data, refetch, isLoading} = useGrades(activePeriodId);

  return (
    <View>
      <GradesList grades={data} isRefreshing={isLoading} onRefresh={refetch} />
    </View>
  );
};

export default GradesSummaryScreen;
