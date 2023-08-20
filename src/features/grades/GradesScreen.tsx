import {View} from 'react-native';
import GradesList from './GradesList/GradesList';
import {useGrades} from './useGrades';

const GradesScreen = () => {
  const grades = useGrades();
  return (
    <View>
      <GradesList grades={grades} />
    </View>
  );
};

export default GradesScreen;
