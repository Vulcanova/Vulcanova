import {createMaterialTopTabNavigator} from '@react-navigation/material-top-tabs';
import GradesSummaryScreen from './GradesSummary/GradesSummaryScreen';
import {useTranslation} from 'react-i18next';
import FinalGradesScreen from './finalGrades/FinalGradesScreen';

export type GradesTabParamList = {
  GradesSummaryScreen: undefined;
  FinalGradesScreen: undefined;
};

const GradesTab = createMaterialTopTabNavigator();

const GradesScreen = () => {
  const {t} = useTranslation('gradesScreen');
  return (
    <GradesTab.Navigator>
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
  );
};

export default GradesScreen;
