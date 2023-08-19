/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 */

import React, {ReactElement} from 'react';
import {useColorScheme} from 'react-native';
import {NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import IntroScreen from './features/auth/IntroScreen';
import {i18nInit} from 'common/i18n/i18n';
import defaultTheme from './themes/defaultTheme';
import QrScannerScreen from './features/auth/qrScanning/QrScannerScreen';
import QrPinScreen from './features/auth/qrScanning/QrPinScreen';
import {AuthQrData} from './features/auth/qrScanning/qrHelper';
import ManualSignInScreen from './features/auth/manualSignIn/ManualSignInScreen';

i18nInit();

export type StackParamList = {
  Intro: undefined;
  QrScanner: undefined;
  QrPinScreen: {qrData: AuthQrData};
  ManualSignInScreen: undefined;
};

const Stack = createNativeStackNavigator<StackParamList>();

function App(): ReactElement {
  const isDarkMode = useColorScheme() === 'dark';

  return (
    <NavigationContainer
      theme={isDarkMode ? defaultTheme.dark : defaultTheme.light}>
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
      </Stack.Navigator>
    </NavigationContainer>
  );
}

export default App;
