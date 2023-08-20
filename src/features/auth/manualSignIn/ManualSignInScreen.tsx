import {StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import {useTranslation} from 'react-i18next';
import {useState} from 'react';
import Button from 'common/components/Button';
import {getInstanceUrl} from 'common/uonet/api/instanceUrlProvider';
import {authenticate} from '../authUtility';
import TextInput from 'common/components/TextInput';
import {getCertThumbprint} from 'common/uonet/crypto/certHelper';
import {useNavigation} from '@react-navigation/native';
import {StackNavigationProp} from '@react-navigation/stack';
import {StackParamList} from '../../../App';
import {useAccountsManagement} from '../useAccountsManagement';
import {storeIdentity} from '../clientIdentityStore';

type ManualSignInScreenNavigationProp = StackNavigationProp<
  StackParamList,
  'Intro'
>;

const ManualSignInScreen = () => {
  const [token, setToken] = useState('');
  const [symbol, setSymbol] = useState('');
  const [pin, setPin] = useState('');

  const navigation = useNavigation<ManualSignInScreenNavigationProp>();
  const {t} = useTranslation(['manualSignInScreen', 'common']);

  const {addAccount} = useAccountsManagement();

  const handleAddAccountPress = async () => {
    const apiUrl = await getInstanceUrl(token, symbol);
    const {account, students, identity} = await authenticate(
      token,
      pin,
      apiUrl!,
    );

    const identityThumbprint = getCertThumbprint(identity.certificate);
    await storeIdentity(identity);
    addAccount(account, students, identityThumbprint);
    navigation.replace('GradesScreen');
  };

  return (
    <View style={styles.root}>
      <View style={styles.textsContainer}>
        <Typography variant="largeTitleEmphasized">
          {t('manualSignInScreen:title')}
        </Typography>
        <Typography variant="subhead">
          {t('manualSignInScreen:subheading')}
        </Typography>
      </View>
      <View style={styles.fieldsContainer}>
        <TextInput
          onChangeText={setToken}
          placeholder="Token"
          autoCapitalize="characters"
        />
        <TextInput onChangeText={setSymbol} placeholder="Symbol" />
        <TextInput
          onChangeText={setPin}
          placeholder="Pin"
          keyboardType="number-pad"
        />
      </View>
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
  fieldsContainer: {
    rowGap: 4,
  },
});

export default ManualSignInScreen;
