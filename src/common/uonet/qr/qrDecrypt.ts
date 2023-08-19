import quickCrypto from 'react-native-quick-crypto';
import {Buffer} from '@craftzdog/react-native-buffer';

const QR_KEY = 'tDVS4ykCBBAeN33h';

// https://github.com/wulkanowy/qr/blob/master/node/index.js
export const decryptQrContent = (qrCode: string): string => {
  const aes = quickCrypto.createDecipheriv(
    'aes-128-ecb',
    Buffer.from(QR_KEY, 'utf-8'),
    Buffer.alloc(0),
  );
  let decrypted = aes.update(qrCode, 'base64', 'utf-8');
  decrypted += aes.final('utf-8');
  return decrypted as string;
};
