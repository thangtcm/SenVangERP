using System.Net;

namespace Application.Bases
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(List<T> data) => Data = data;
        public List<T>? Data { get; set; }

        internal PaginatedResult(bool succeeded, List<T>? data = default, string? messages = null, long count = 0, int page = 1, int pageSize = 10, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            StatusCode = statusCode;
            Message = messages;
        }

        internal PaginatedResult(bool succeeded, string? messages = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = messages;
        }
        public static PaginatedResult<T> Success(List<T> data, long count, int page, int pageSize, string? message = null)
        {
            return new(true, data, message, count, page, pageSize, HttpStatusCode.OK);
        }
        public static PaginatedResult<T> InternalServerError(List<T> data, long count, int page, int pageSize)
        {
            return new(false, [], null, count, page, pageSize, HttpStatusCode.InternalServerError);
        }
        public static PaginatedResult<T> Unauthorized(List<T> data, long count, int page, int pageSize)
        {
            return new(false, [], null, count, page, pageSize, HttpStatusCode.Unauthorized);
        }
        public static PaginatedResult<T> BadRequest(List<T> data, long count, int page, int pageSize)
        {
            return new(false, [], null, count, page, pageSize, HttpStatusCode.BadRequest);
        }
        public HttpStatusCode StatusCode { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public long TotalCount { get; set; }

        public object Meta { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public string? Message { get; set; }

        public bool Succeeded { get; set; }
    }
}
