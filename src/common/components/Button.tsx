import {StyleSheet, TouchableOpacity} from 'react-native';
import {Theme, useTheme} from '@react-navigation/native';
import Typography from 'common/components/Typography';

export interface ButtonProps {
  title: string;
  onPress?(): void;
}

const Button = ({title, onPress}: ButtonProps) => {
  const theme = useTheme();
  const styles = makeStyles(theme);

  return (
    <TouchableOpacity style={styles.root} onPress={onPress}>
      <Typography color="white" variant="body">
        {title}
      </Typography>
    </TouchableOpacity>
  );
};

const makeStyles = ({colors}: Theme) =>
  StyleSheet.create({
    root: {
      borderRadius: 50,
      backgroundColor: colors.primary,
      justifyContent: 'center',
      alignItems: 'center',
      padding: 10,
    },
    text: {
      color: 'white',
    },
  });

export default Button;
