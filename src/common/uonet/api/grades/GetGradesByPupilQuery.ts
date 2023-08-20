import {PaginatedApiQuery} from 'common/uonet/api/PaginatedApiQuery';

export interface GetGradesByPupilQuery extends PaginatedApiQuery {
  unitId: number;
  pupilId: number;
  periodId: number;
  lastSyncDate: Date;
}

export module GetGradesByPupilQuery {
  export const API_ENDPOINT = 'mobile/grade/byPupil';
}
