import {Theme} from '@react-navigation/native';

export interface AppThemeSet {
  light: AppTheme;
  dark: AppTheme;
}

export interface AppTheme {
  dark: boolean;
  colors: AppThemeColors;
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
