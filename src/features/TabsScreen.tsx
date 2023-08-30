import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';
import GradesScreen from './grades/GradesScreen';
import StudentAvatarButton from './auth/StudentAvatarButton';
import {StudentProvider} from './auth/StudentContext';
import GradesTabIcon from './grades/GradesTabIcon';

export type TabParamList = {
  Grades: {periodId?: number};
};

const Tab = createBottomTabNavigator<TabParamList>();

const TabsScreen = () => {
  return (
    <StudentProvider>
      <Tab.Navigator screenOptions={{headerRight: StudentAvatarButton}}>
        <Tab.Screen
          name="Grades"
          component={GradesScreen}
          options={{
            tabBarIcon: GradesTabIcon,
          }}
        />
      </Tab.Navigator>
    </StudentProvider>
  );
};

export default TabsScreen;
