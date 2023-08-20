import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import GradesScreen from './grades/GradesScreen';
import StudentAvatarButton from './auth/StudentAvatarButton';
import {StudentProvider} from './auth/StudentContext';

export type TabParamList = {
  Grades: undefined;
};

const Tab = createBottomTabNavigator<TabParamList>();

const TabsScreen = () => {
  return (
    <StudentProvider>
      <Tab.Navigator screenOptions={{headerRight: StudentAvatarButton}}>
        <Tab.Screen name="Grades" component={GradesScreen} />
      </Tab.Navigator>
    </StudentProvider>
  );
};

export default TabsScreen;
