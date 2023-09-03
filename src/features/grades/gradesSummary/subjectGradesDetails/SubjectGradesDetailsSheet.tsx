import {SubjectGrades} from '../types';
import {BottomSheetFlatList, BottomSheetModal} from '@gorhom/bottom-sheet';
import {forwardRef, useImperativeHandle, useRef} from 'react';
import {StyleSheet} from 'react-native';
import SubjectGradesDetailsSheetGradeItem from './SubjectGradesDetailsSheetGradeItem';
import {AppTheme} from '../../../../themes/types';
import {useAppTheme} from 'common/ui/useAppTheme';

export interface SubjectGradesDetailsSheetHandle {
  present(subjectGrades: SubjectGrades): void;
}

export interface SubjectGradesDetailsSheetProps {}

const SubjectGradesDetailsSheet = forwardRef<
  SubjectGradesDetailsSheetHandle,
  SubjectGradesDetailsSheetProps
>((_, ref) => {
  const sheetRef = useRef<BottomSheetModal>(null);

  const snapPoints = ['50%', '90%'];

  const handlePresent = (subjectGrades: SubjectGrades) => {
    sheetRef.current?.present(subjectGrades);
  };

  useImperativeHandle(ref, () => ({
    present: handlePresent,
  }));

  const theme = useAppTheme();
  const styles = makeStyles(theme);

  return (
    <BottomSheetModal
      snapPoints={snapPoints}
      backgroundStyle={styles.background}
      handleIndicatorStyle={styles.handleIndicator}
      enablePanDownToClose
      ref={sheetRef}>
      {({data}: {data: SubjectGrades}) => (
        <BottomSheetFlatList
          keyExtractor={item => item.id.toString()}
          data={data.grades}
          renderItem={({item}) => (
            <SubjectGradesDetailsSheetGradeItem grade={item} />
          )}
        />
      )}
    </BottomSheetModal>
  );
});

const makeStyles = ({colors}: AppTheme) =>
  StyleSheet.create({
    background: {
      backgroundColor: colors.panel,
    },
    handleIndicator: {
      backgroundColor: colors.tertiaryText,
    },
  });

export default SubjectGradesDetailsSheet;
