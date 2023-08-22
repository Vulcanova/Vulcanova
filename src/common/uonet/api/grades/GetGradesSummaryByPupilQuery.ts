import {PaginatedApiQuery} from 'common/uonet/api/PaginatedApiQuery';

export interface GetGradesSummaryByPupilQuery extends PaginatedApiQuery {
  unitId: number;
  pupilId: number;
  periodId: number;
}

export module GetGradesSummaryByPupilQuery {
  export const API_ENDPOINT = 'mobile/grade/summary/byPupil';
}
