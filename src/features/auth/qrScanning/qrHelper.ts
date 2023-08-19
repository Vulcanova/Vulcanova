export interface AuthQrData {
  apiAddress: string;
  token: string;
}

export const parseQrContent = (content: string): AuthQrData => {
  const values = content.split('#');

  if (values.length != 4) {
    throw new Error('Invalid QR code ' + content);
  }

  return {
    apiAddress: values[1],
    token: values[2],
  };
};
