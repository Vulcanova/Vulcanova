import {
  StyleSheet,
  TextInput as TextInputBase,
  TextInputProps as TextInputBaseProps,
  View,
} from 'react-native';
import {Theme, useTheme} from '@react-navigation/native';
import {forwardRef} from 'react';

export interface TextInputProps extends TextInputBaseProps {}

const TextInput = forwardRef<TextInputBase, TextInputProps>(
  ({style, ...rest}: TextInputProps, ref) => {
    const theme = useTheme();
    const styles = makeStyles(theme);

    return (
      <View style={styles.root}>
        <TextInputBase
          underlineColorAndroid="transparent"
          style={[style, styles.input]}
          ref={ref}
          {...rest}
        />
      </View>
    );
  },
);

const makeStyles = ({colors}: Theme) =>
  StyleSheet.create({
    root: {
      borderWidth: 1,
      borderColor: colors.border,
      borderRadius: 4,
    },
    input: {
      padding: 10,
    },
  });

export default TextInput;
