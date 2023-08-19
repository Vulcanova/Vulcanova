import {Text, TextProps} from 'react-native';
import {iOSUIKit} from 'react-native-typography';
import {useTheme} from '@react-navigation/native';

type Variant = keyof typeof iOSUIKit;

export interface TypographyProps extends TextProps {
  variant?: Variant;
  color?: string;
}

const Typography = ({
  children,
  variant = 'body',
  color,
  style,
  onPress,
}: TypographyProps) => {
  const theme = useTheme();

  if (!color) {
    color = theme.colors.text;
  }

  return (
    <Text
      style={[
        iOSUIKit[variant],
        {
          color: color,
        },
        style,
      ]}
      onPress={onPress}>
      {children}
    </Text>
  );
};

export default Typography;
