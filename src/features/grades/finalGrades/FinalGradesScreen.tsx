import {useFinalGrades} from './useFinalGrades';
import FinalGradesList from './FinalGradesList';
import {usePeriod} from '../PeriodContext';

const FinalGradesScreen = () => {
  const {activePeriodId} = usePeriod();
  const finalGrades = useFinalGrades(activePeriodId);

  return <FinalGradesList finalGrades={finalGrades} />;
};

export default FinalGradesScreen;
