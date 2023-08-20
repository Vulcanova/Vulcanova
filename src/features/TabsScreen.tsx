import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import GradesScreen from './grades/GradesScreen';
import StudentAvatarButton from './auth/StudentAvatarButton';

export type TabParamList = {
  Grades: undefined;
};

const Tab = createBottomTabNavigator<TabParamList>();

const TabsScreen = () => {
  return (
    <Tab.Navigator screenOptions={{headerRight: StudentAvatarButton}}>
      <Tab.Screen name="Grades" component={GradesScreen} />
    </Tab.Navigator>
  );
};

export default TabsScreen;
