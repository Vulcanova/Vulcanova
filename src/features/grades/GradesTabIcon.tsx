import Icon from 'react-native-vector-icons/Ionicons';
import React from 'react';

interface Props {
  color: string;
  size: number;
}

const GradesTabIcon = ({color, size}: Props) => {
  return <Icon name="medal-outline" size={size} color={color} />;
};

export default GradesTabIcon;
