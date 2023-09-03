/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 */

import React, {ReactElement} from 'react';
import {StyleSheet, useColorScheme} from 'react-native';
import {
  NavigationContainer,
  NavigatorScreenParams,
} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import IntroScreen from './features/auth/IntroScreen';
import {i18nInit} from 'common/i18n/i18n';
import defaultTheme from './themes/defaultTheme';
import QrScannerScreen from './features/auth/qrScanning/QrScannerScreen';
import QrPinScreen from './features/auth/qrScanning/QrPinScreen';
import {AuthQrData} from './features/auth/qrScanning/qrHelper';
import ManualSignInScreen from './features/auth/manualSignIn/ManualSignInScreen';
import {AppRealmContext} from 'common/data/AppRealmContext';
import TabsScreen, {TabParamList} from './features/TabsScreen';
import {QueryClient, QueryClientProvider} from 'react-query';
import {GestureHandlerRootView} from 'react-native-gesture-handler';
import {BottomSheetModalProvider} from '@gorhom/bottom-sheet';
i18nInit();

export type StackParamList = {
  Intro: undefined;
  QrScanner: undefined;
  QrPinScreen: {qrData: AuthQrData};
  ManualSignInScreen: undefined;
  Tabs: NavigatorScreenParams<TabParamList>;
};

const Stack = createNativeStackNavigator<StackParamList>();

const queryClient = new QueryClient();

function App(): ReactElement {
  const isDarkMode = useColorScheme() === 'dark';

  return (
    <GestureHandlerRootView style={styles.gestureHandler}>
      <QueryClientProvider client={queryClient}>
        <AppRealmContext.RealmProvider>
          <NavigationContainer
            theme={isDarkMode ? defaultTheme.dark : defaultTheme.light}>
            <BottomSheetModalProvider>
              <Stack.Navigator>
                <Stack.Screen
                  name="Intro"
                  component={IntroScreen}
                  options={{headerTitle: '', headerTransparent: true}}
                />
                <Stack.Screen
                  name="QrScanner"
                  component={QrScannerScreen}
                  options={{headerTitle: '', headerTransparent: true}}
                />
                <Stack.Screen
                  name="QrPinScreen"
                  component={QrPinScreen}
                  options={{headerTitle: '', headerTransparent: true}}
                />
                <Stack.Screen
                  name="ManualSignInScreen"
                  component={ManualSignInScreen}
                  options={{headerTitle: '', headerTransparent: true}}
                />
                <Stack.Screen
                  name="Tabs"
                  component={TabsScreen}
                  options={{headerShown: false}}
                />
              </Stack.Navigator>
            </BottomSheetModalProvider>
          </NavigationContainer>
        </AppRealmContext.RealmProvider>
      </QueryClientProvider>
    </GestureHandlerRootView>
  );
}

const styles = StyleSheet.create({
  gestureHandler: {
    flex: 1,
  },
});

export default App;
