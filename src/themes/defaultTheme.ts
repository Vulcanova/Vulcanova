import {AppTheme} from './types';

const defaultTheme: AppTheme = {
  light: {
    dark: false,
    colors: {
      primary: '#007aff',
      background: 'white',
      card: '#f7f7f9',
      text: 'black',
      border: '#F2F2F7',
      notification: 'white',
      translucentPrimary: '#DFF1FF',
      secondaryText: '#AAAAAA',
      dashboardControl: '#FFF',
      dashboardBackground: '#F3F3F5',
      panel: 'white',
      warning: '#FF9500',
      error: '#FF3B30',
      errorColorDarker: '#ffE9E9',
      header: '#E5E5EA',
    },
  },
  dark: {
    dark: true,
    colors: {
      primary: '#0a84ff',
      background: 'black',
      card: '#131313',
      text: 'white',
      border: '#1C1C1C',
      notification: 'black',
      translucentPrimary: '#7EC7FF',
      secondaryText: '#AAAAAA',
      dashboardControl: '#131313',
      dashboardBackground: '#000',
      panel: '#131313',
      warning: '#FF9F0A',
      error: '#FF453A',
      errorColorDarker: '#6e0600',
      header: '#2C2C2E',
    },
  },
};

export default defaultTheme;
