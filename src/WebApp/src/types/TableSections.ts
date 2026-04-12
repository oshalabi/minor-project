export type TableSections = {
  header: string;
  stylingClass?: string;
  columns: Array<{
    field: string;
    displayName: string;
    editable?: boolean;
  }>;
};
