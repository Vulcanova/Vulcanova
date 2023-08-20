import {StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import {useStudent} from './StudentContext';
import {AppTheme} from '../../themes/types';
import {useAppTheme} from 'common/ui/useAppTheme';

const StudentAvatarButton = () => {
  const theme = useAppTheme();
  const styles = makeStyles(theme);

  const {activeStudent} = useStudent();

  const text =
    activeStudent === undefined
      ? '?'
      : activeStudent.pupil.firstName[0] + activeStudent.pupil.surname[0];

  return (
    <View style={styles.root}>
      <Typography variant="subhead" color={theme.colors.primary}>
        {text}
      </Typography>
    </View>
  );
};

const makeStyles = ({colors}: AppTheme) =>
  StyleSheet.create({
    root: {
      borderRadius: 99,
      backgroundColor: colors.translucentPrimary,
      alignItems: 'center',
      justifyContent: 'center',
      width: 38,
      height: 38,
      marginRight: 4,
    },
  });

export default StudentAvatarButton;
