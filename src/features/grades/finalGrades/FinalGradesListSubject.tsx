import {FinalGradesEntry} from './FinalGradesEntry.schema';
import {StyleSheet, View} from 'react-native';
import {useAppTheme} from 'common/ui/useAppTheme';
import Typography from 'common/components/Typography';
import {useTranslation} from 'react-i18next';
import ListDivider from 'common/components/ListDivider';

export interface FinalGradesListSubjectProps {
  finalGradesEntry: FinalGradesEntry;
}

const FinalGradesListSubject = ({
  finalGradesEntry,
}: FinalGradesListSubjectProps) => {
  const {t} = useTranslation('gradesScreen');
  const theme = useAppTheme();

  return (
    <View>
      <View style={styles.root}>
        <Typography>{finalGradesEntry.subject.name}</Typography>
        <View style={styles.gradeRow}>
          <Typography variant="subhead">
            {t('summaryScreen.predictedGrade')}
          </Typography>
          <Typography variant="subhead">
            {finalGradesEntry.predictedGrade}
          </Typography>
        </View>
        <View style={styles.gradeRow}>
          <Typography variant="subhead" color={theme.colors.primary}>
            {t('summaryScreen.finalGrade')}
          </Typography>
          <Typography variant="subhead" color={theme.colors.primary}>
            {finalGradesEntry.finalGrade}
          </Typography>
        </View>
      </View>
      <ListDivider />
    </View>
  );
};

const styles = StyleSheet.create({
  root: {
    paddingVertical: 8,
    paddingHorizontal: 16,
  },
  gradeRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
});

export default FinalGradesListSubject;
