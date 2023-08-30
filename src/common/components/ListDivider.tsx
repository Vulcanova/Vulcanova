import {useAppTheme} from 'common/ui/useAppTheme';
import {AppTheme} from '../../themes/types';
import {StyleSheet, View} from 'react-native';

const ListDivider = () => {
  const theme = useAppTheme();
  const styles = makeStyles(theme);

  return <View style={styles.root} />;
};

const makeStyles = ({colors}: AppTheme) =>
  StyleSheet.create({
    root: {
      backgroundColor: colors.border,
      height: 1,
      width: '100%',
    },
  });

export default ListDivider;
