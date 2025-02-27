export interface CommandeDto {
  id: number;
  dateCommande: string;
  status: string;
  commandePizzas: {
    pizzaNom: string;
    quantite: number;
  }[];
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
}
