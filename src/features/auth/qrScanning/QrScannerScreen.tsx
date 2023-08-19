import QRCodeScanner from 'react-native-qrcode-scanner';
import {BarCodeReadEvent, RNCamera} from 'react-native-camera';
import {parseQrContent} from './qrHelper';
import {StackNavigationProp} from '@react-navigation/stack';
import {StackParamList} from '../../../App';
import {useFocusEffect, useNavigation} from '@react-navigation/native';
import {decryptQrContent} from 'common/uonet/qr/qrDecrypt';
import {useCallback, useState} from 'react';

type QrScannerScreenNavigationProp = StackNavigationProp<
  StackParamList,
  'QrScanner'
>;

const QrScannerScreen = () => {
  const [isActive, setIsActive] = useState(false);
  const navigation = useNavigation<QrScannerScreenNavigationProp>();

  useFocusEffect(
    useCallback(() => {
      setIsActive(true);

      return () => setIsActive(false);
    }, []),
  );

  const handleRead = (e: BarCodeReadEvent) => {
    const content = decryptQrContent(e.data);
    const qrData = parseQrContent(content);

    navigation.navigate('QrPinScreen', {qrData});
  };

  return isActive ? <QRCodeScanner onRead={handleRead} /> : null;
};

export default QrScannerScreen;
