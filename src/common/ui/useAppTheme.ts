import {useTheme} from '@react-navigation/native';
import {AppTheme} from '../../themes/types';

export const useAppTheme = () => {
  return useTheme() as AppTheme;
};
