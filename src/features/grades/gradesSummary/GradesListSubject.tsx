import {SubjectGrades} from './types';
import React from 'react';
import {StyleSheet, TouchableHighlight, View} from 'react-native';
import Typography from 'common/components/Typography';
import GradesListSubjectGrade from './GradesListSubjectGrade';
import Icon from 'react-native-vector-icons/Ionicons';
import {useAppTheme} from 'common/ui/useAppTheme';
import ListDivider from 'common/components/ListDivider';

export interface GradesListSubjectProps {
  subject: SubjectGrades;
  onPress(): void;
}

const GradesListSubject = ({subject, onPress}: GradesListSubjectProps) => {
  const theme = useAppTheme();

  return (
    <View>
      <TouchableHighlight onPress={onPress}>
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
      </TouchableHighlight>
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
