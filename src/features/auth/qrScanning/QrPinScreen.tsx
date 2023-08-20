import {StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import {useTranslation} from 'react-i18next';
import {extractInstanceUrlFromRequestUrl} from 'common/uonet/api/instanceUrlProvider';
import {StackParamList} from '../../../App';
import {NativeStackScreenProps} from '@react-navigation/native-stack';
import {authenticate} from '../authUtility';
import {useState} from 'react';
import Button from 'common/components/Button';
import TextInput from 'common/components/TextInput';
import {useAccountsManagement} from '../useAccountsManagement';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {storeIdentity} from '../clientIdentityStore';

type QrPinScreenNavigationProp = NativeStackScreenProps<
  StackParamList,
  'QrPinScreen'
>;

const QrPinScreen = ({route, navigation}: QrPinScreenNavigationProp) => {
  const [pin, setPin] = useState('');
  const {t} = useTranslation(['qrPinScreen', 'common']);
  const {addAccount} = useAccountsManagement();

  const handleAddAccountPress = async () => {
    const qr = route.params.qrData;
    const instanceUrl = extractInstanceUrlFromRequestUrl(qr.apiAddress);
    const {account, students, identity} = await authenticate(
      qr.token,
      pin,
      instanceUrl,
    );

    const identityThumbprint = getCertThumbprint(identity.certificate);
    addAccount(account, students, identityThumbprint);
    await storeIdentity(identity);
    navigation.replace('GradesScreen');
  };

  return (
    <View style={styles.root}>
      <View style={styles.textsContainer}>
        <Typography variant="largeTitleEmphasized">
          {t('qrPinScreen:enterPinCode')}
        </Typography>
        <Typography variant="subhead">
          {t('qrPinScreen:enterPinCodeSubheading')}
        </Typography>
      </View>
      <TextInput
        onChangeText={setPin}
        placeholder="Pin"
        keyboardType="number-pad"
      />
      <Button title={t('common:add')} onPress={handleAddAccountPress} />
    </View>
  );
};

const styles = StyleSheet.create({
  root: {
    alignContent: 'center',
    justifyContent: 'center',
    height: '100%',
    padding: 32,
    rowGap: 30,
  },
  textsContainer: {
    rowGap: 4,
  },
});

export default QrPinScreen;
