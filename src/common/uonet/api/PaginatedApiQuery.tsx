export interface PaginatedApiQuery<T = number> {
  pageSize: number;
  lastId: T;
}
