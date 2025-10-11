export interface ErrorModel {
  code: string | null;
  message: string | null;
}

export interface ErrorResponse {
  errors: ErrorModel[] | null;
}
