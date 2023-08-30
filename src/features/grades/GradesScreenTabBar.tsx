import PeriodPicker from 'common/components/PeriodPicker';
import {
  MaterialTopTabBar,
  MaterialTopTabBarProps,
} from '@react-navigation/material-top-tabs';
import {View} from 'react-native';
import {useStudent} from '../auth/StudentContext';
import {Period} from '../auth/Student.schema';
import {usePeriod} from './PeriodContext';

export interface GradesScreenTabBarProps extends MaterialTopTabBarProps {}

const GradesScreenTabBar = (props: GradesScreenTabBarProps) => {
  const {activeStudent} = useStudent();
  const {activePeriodId, setActivePeriodId} = usePeriod();

  const handlePeriodChange = (period: Period) => {
    setActivePeriodId(period.id);
  };

  return (
    <View>
      <PeriodPicker
        periods={activeStudent.periods}
        selectedPeriod={
          activeStudent.periods.find(p => p.id === activePeriodId)!
        }
        onChange={handlePeriodChange}
      />
      <MaterialTopTabBar {...props} />
    </View>
  );
};

export default GradesScreenTabBar;
