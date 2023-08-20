import {GradePayload} from 'common/uonet/api/grades/GradePayload';
import Typography from 'common/components/Typography';
import {useAppTheme} from 'common/ui/useAppTheme';

export interface GradesListSubjectGradeProps {
  grade: GradePayload;
}

const GradesListSubjectGrade = ({grade}: GradesListSubjectGradeProps) => {
  const theme = useAppTheme();

  const color =
    grade.column.color === 0
      ? theme.colors.text
      : `#${grade.column.color.toString(16)}`;

  return (
    <Typography variant="subhead" color={color}>
      {grade.content}
    </Typography>
  );
};

export default GradesListSubjectGrade;
