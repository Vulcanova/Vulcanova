import {useFinalGrades} from './useFinalGrades';
import FinalGradesList from './FinalGradesList';

const FinalGradesScreen = () => {
  const finalGrades = useFinalGrades();

  return <FinalGradesList finalGrades={finalGrades} />;
};

export default FinalGradesScreen;
