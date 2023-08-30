import {View} from 'react-native';
import GradesList from './GradesList';
import {useGrades} from '../useGrades';
import {usePeriod} from '../PeriodContext';

const GradesSummaryScreen = () => {
  const {activePeriodId} = usePeriod();
  const grades = useGrades(activePeriodId);

  return (
    <View>
      <GradesList grades={grades} />
    </View>
  );
};

export default GradesSummaryScreen;
