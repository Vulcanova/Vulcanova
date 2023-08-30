import {Period} from '../../features/auth/Student.schema';
import {StyleSheet, View} from 'react-native';
import {AppTheme} from '../../themes/types';
import {useAppTheme} from 'common/ui/useAppTheme';
import Typography from 'common/components/Typography';
import Icon from 'react-native-vector-icons/Ionicons';
import React from 'react';

export interface PeriodPickerProps {
  periods: Realm.List<Period>;
  selectedPeriod: Period;
  onChange(period: Period): void;
}

const PeriodPicker = ({
  selectedPeriod,
  periods,
  onChange,
}: PeriodPickerProps) => {
  const theme = useAppTheme();
  const styles = makeStyles(theme);

  const orderedPeriods = [...periods].sort((a, z) => +a.start - +z.start);

  const allPeriodsInYear = orderedPeriods.filter(
    p => p.level === selectedPeriod.level,
  );

  const firstPeriod = allPeriodsInYear[0];
  const lastPeriod = allPeriodsInYear.at(-1);

  const indexOfSelected = orderedPeriods.findIndex(
    p => p.id === selectedPeriod.id,
  );

  const hasPrevious = indexOfSelected !== 0;
  const hasNext = indexOfSelected !== periods.length - 1;

  return (
    <View style={styles.root}>
      <Icon
        name="chevron-back-outline"
        size={22}
        color={theme.colors.tertiaryText}
        onPress={() => onChange(orderedPeriods[indexOfSelected - 1])}
        {...(hasPrevious ? {} : {style: styles.hide})}
      />
      <Typography style={styles.text}>
        {firstPeriod.start.getFullYear()}/{lastPeriod!.end.getFullYear()} –{' '}
        {selectedPeriod.number}
      </Typography>
      <Icon
        name="chevron-forward-outline"
        size={22}
        color={theme.colors.tertiaryText}
        onPress={() => onChange(orderedPeriods[indexOfSelected + 1])}
        {...(hasNext ? {} : {style: styles.hide})}
      />
    </View>
  );
};

const makeStyles = ({colors}: AppTheme) =>
  StyleSheet.create({
    root: {
      backgroundColor: colors.card,
      borderRadius: 16,
      flexDirection: 'row',
      justifyContent: 'center',
    },
    text: {
      paddingHorizontal: 8,
    },
    hide: {
      opacity: 0,
      pointerEvents: 'none',
    },
  });

export default PeriodPicker;
