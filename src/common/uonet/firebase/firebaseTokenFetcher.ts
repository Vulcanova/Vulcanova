import axios, {AxiosResponse} from 'axios';

export const fetchFirebaseToken = async (): Promise<string> => {
  const aid = '4609707972546570896:3626695765779152704';
  const device = aid.split(':')[0];

  const app = 'pl.edu.vulcan.hebe';

  const headers = {
    Authorization: `AidLogin ${aid}`,
    'User-Agent': 'Android-GCM/1.5',
    app: app,
  };

  const data = new URLSearchParams();
  data.append('sender', '987828170337');
  data.append('X-Scope', '*');
  data.append('X-gmp_app_id', '1:987828170337:android:ac97431a0a4578c3');
  data.append('app', app);
  data.append('device', device);

  const response: AxiosResponse<string> = await axios.post(
    'https://android.clients.google.com/c2dm/register3',
    data.toString(),
    {
      headers: headers,
    },
  );

  const responseBody = response.data;
  const responseParts = responseBody.split('=');
  return responseParts[1];
};
