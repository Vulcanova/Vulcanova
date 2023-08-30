import {SubjectGrades} from './types';
import React from 'react';
import {StyleSheet, View} from 'react-native';
import Typography from 'common/components/Typography';
import GradesListSubjectGrade from './GradesListSubjectGrade';
import Icon from 'react-native-vector-icons/Ionicons';
import {useAppTheme} from 'common/ui/useAppTheme';
import ListDivider from 'common/components/ListDivider';

export interface GradesListSubjectProps {
  subject: SubjectGrades;
}

const GradesListSubject = ({subject}: GradesListSubjectProps) => {
  const theme = useAppTheme();

  return (
    <View>
      <View style={styles.contentWrapper}>
        <View style={styles.subjectWrapper}>
          <Typography variant="body">{subject.subjectName}</Typography>
          <View style={styles.gradesWrapper}>
            {subject.grades.map(g => (
              <GradesListSubjectGrade grade={g} key={g.id} />
            ))}
          </View>
        </View>
        <View>
          <Icon
            name="chevron-forward-outline"
            size={22}
            color={theme.colors.tertiaryText}
          />
        </View>
      </View>
      <ListDivider />
    </View>
  );
};

const styles = StyleSheet.create({
  contentWrapper: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 8,
    paddingHorizontal: 16,
  },
  subjectWrapper: {
    flexShrink: 1,
  },
  gradesWrapper: {
    flexWrap: 'wrap',
    flexDirection: 'row',
    columnGap: 6,
  },
});

export default GradesListSubject;
