import {StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import {Grade} from '../../Grade.schema';
import {useTranslation} from 'react-i18next';
import ListDivider from 'common/components/ListDivider';
import {useAppTheme} from 'common/ui/useAppTheme';

export interface SubjectGradesDetailsSheetGradeItem {
  grade: Grade;
}

const SubjectGradesDetailsSheetGradeItem = ({
  grade,
}: SubjectGradesDetailsSheetGradeItem) => {
  const theme = useAppTheme();
  const {t} = useTranslation('gradesScreen');

  let dateString = t('subjectGradesDetails.dateCreated', {
    date: grade.dateCreated.toLocaleDateString(),
  });

  if (
    grade.dateModify !== undefined &&
    +grade.dateCreated !== +grade.dateModify
  ) {
    dateString += ` ${t('subjectGradesDetails.dateModified', {
      date: grade.dateModify.toLocaleDateString(),
    })}`;
  }

  const color =
    grade.column.color === 0
      ? theme.colors.text
      : `#${grade.column.color.toString(16)}`;

  return (
    <View>
      <View style={styles.contentContainer}>
        <View style={styles.gradeValueWrapper}>
          <Typography variant="title3Emphasized" color={color}>
            {grade.content}
          </Typography>
        </View>
        <View style={styles.gradeInformationWrapper}>
          <Typography numberOfLines={2}>{grade.column.name}</Typography>
          <Typography variant="subhead" color={theme.colors.secondaryText}>
            {grade.contentRaw}
          </Typography>
          <Typography variant="subhead" color={theme.colors.secondaryText}>
            {dateString}
          </Typography>
        </View>
      </View>
      <ListDivider />
    </View>
  );
};

const styles = StyleSheet.create({
  contentContainer: {
    flexDirection: 'row',
    paddingRight: 12,
    paddingVertical: 8,
  },
  gradeValueWrapper: {
    justifyContent: 'center',
    alignItems: 'center',
    width: 60,
  },
  gradeInformationWrapper: {
    flexShrink: 1,
  },
});

export default SubjectGradesDetailsSheetGradeItem;
