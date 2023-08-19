import {Theme} from '@react-navigation/native';

export interface AppTheme {
  light: {
    dark: boolean;
    colors: AppThemeColors;
  };
  dark: {
    dark: boolean;
    colors: AppThemeColors;
  };
}

export type AppThemeColors = Theme['colors'] & {
  translucentPrimary: string;
  secondaryText: string;
  dashboardControl: string;
  dashboardBackground: string;
  panel: string;
  warning: string;
  error: string;
  errorColorDarker: string;
  header: string;
};
