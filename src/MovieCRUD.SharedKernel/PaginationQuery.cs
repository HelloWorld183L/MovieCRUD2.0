namespace MovieCRUD.SharedKernel
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 5;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public static PaginationQuery CreateQuery(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return new PaginationQuery();
            }
            return new PaginationQuery(pageNumber, pageSize);
        }
    }
}
