import {Linking, StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import {useTranslation} from 'react-i18next';
import Button from 'common/components/Button';
import {CommonActions, useNavigation, useTheme} from '@react-navigation/native';
import type {StackNavigationProp} from '@react-navigation/stack';
import {StackParamList} from '../../App';
import {Student} from './Student.schema';
import {useEffect} from 'react';
import {useRealm} from 'common/data/AppRealmContext';

type IntroScreenNavigationProp = StackNavigationProp<StackParamList, 'Intro'>;

const IntroScreen = () => {
  const theme = useTheme();
  const navigation = useNavigation<IntroScreenNavigationProp>();
  const {t} = useTranslation('introScreen');

  const realm = useRealm();

  useEffect(() => {
    const activeStudent = realm.objects(Student).filtered('isActive = true')[0];
    if (activeStudent) {
      navigation.dispatch(
        CommonActions.reset({
          index: 0,
          routes: [{name: 'Tabs', state: {routes: [{name: 'Grades'}]}}],
        }),
      );
    }
  }, [realm, navigation]);

  const handleScanQrCodeClick = async () => {
    navigation.navigate('QrScanner');
  };

  const handleSignInManuallyPress = () => {
    navigation.navigate('ManualSignInScreen');
  };

  const handleGitHubLinkClick = async () => {
    await Linking.openURL('https://github.com/Vulcanova/Vulcanova');
  };

  return (
    <View style={styles.container}>
      <View style={styles.textsContainer}>
        <Typography variant="largeTitleEmphasized">Vulcanova</Typography>
        <Typography variant="subhead">{t('subheading')}</Typography>
      </View>
      <View style={styles.buttonsContainer}>
        <Typography variant="subhead" style={styles.buttonsLabel}>
          {t('addAccount')}
        </Typography>
        <Button title={t('scanQrCode')} onPress={handleScanQrCodeClick} />
        <Button
          title={t('signInManually')}
          onPress={handleSignInManuallyPress}
        />
      </View>
      <Typography
        color={theme.colors.primary}
        variant="subhead"
        onPress={handleGitHubLinkClick}
        style={styles.gitHubLink}>
        {t('gitHubLink')}
      </Typography>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    height: '100%',
    display: 'flex',
    justifyContent: 'center',
    alignContent: 'center',
    padding: 32,
    rowGap: 30,
  },
  textsContainer: {
    rowGap: 4,
  },
  buttonsLabel: {
    textAlign: 'center',
  },
  buttonsContainer: {
    rowGap: 4,
  },
  gitHubLink: {
    textAlign: 'center',
  },
});

export default IntroScreen;
