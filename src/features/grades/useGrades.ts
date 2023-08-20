import {useApiClient} from 'common/data/useApiClient';
import {useEffect, useState} from 'react';
import {GradePayload} from 'common/uonet/api/grades/GradePayload';
import {useStudent} from '../auth/StudentContext';
import {GetGradesByPupilQuery} from 'common/uonet/api/grades/GetGradesByPupilQuery';

export const useGrades = () => {
  const [grades, setGrades] = useState<GradePayload[]>([]);
  const {activeStudent} = useStudent();
  const apiClient = useApiClient();

  useEffect(() => {
    if (
      apiClient === null ||
      activeStudent === null ||
      activeStudent.id === undefined
    ) {
      return;
    }

    const fetch = async () => {
      const request: GetGradesByPupilQuery = {
        unitId: activeStudent.unit.id,
        pupilId: activeStudent.pupil.id,
        pageSize: 500,
        lastId: (Math.pow(2, 32) / 2) * -1,
        lastSyncDate: new Date(1),
        periodId: activeStudent.periods[0].id,
      };

      const response = await apiClient.get<GradePayload[]>(
        GetGradesByPupilQuery.API_ENDPOINT,
        request,
      );

      setGrades(response.envelope);
    };

    fetch();
  }, [activeStudent, apiClient]);

  return grades;
};
