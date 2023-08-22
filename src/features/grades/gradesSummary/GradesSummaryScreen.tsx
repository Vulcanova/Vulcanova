import {View} from 'react-native';
import GradesList from './GradesList';
import {useGrades} from '../useGrades';

const GradesSummaryScreen = () => {
  const grades = useGrades();

  return (
    <View>
      <GradesList grades={grades} />
    </View>
  );
};

export default GradesSummaryScreen;
