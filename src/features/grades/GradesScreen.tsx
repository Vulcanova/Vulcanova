import {createMaterialTopTabNavigator} from '@react-navigation/material-top-tabs';
import {useTranslation} from 'react-i18next';
import FinalGradesScreen from './finalGrades/FinalGradesScreen';
import GradesSummaryScreen from './gradesSummary/GradesSummaryScreen';
import GradesScreenTabBar from './GradesScreenTabBar';
import {PeriodProvider} from './PeriodContext';

export type GradesTabParamList = {
  GradesSummaryScreen: undefined;
  FinalGradesScreen: undefined;
};

const GradesTab = createMaterialTopTabNavigator<GradesTabParamList>();

const GradesScreen = () => {
  const {t} = useTranslation('gradesScreen');

  return (
    <PeriodProvider>
      <GradesTab.Navigator tabBar={GradesScreenTabBar}>
        <GradesTab.Screen
          name="GradesSummaryScreen"
          component={GradesSummaryScreen}
          options={{title: t('tabs.summary')}}
        />
        <GradesTab.Screen
          name="FinalGradesScreen"
          component={FinalGradesScreen}
          options={{title: t('tabs.final')}}
        />
      </GradesTab.Navigator>
    </PeriodProvider>
  );
};

export default GradesScreen;
