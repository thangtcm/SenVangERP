using System.Net;

namespace Application.Bases
{
    public class BaseResponseHandler()
    {
        public BaseResponse<T> Deleted<T>() => new BaseResponse<T>()
        {
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = "Đã xóa dữ liệu thành công"
        };

        public BaseResponse<T> Success<T>(T entity, string? message = "", object? meta = null)
        {
            if (entity is string)
            {
                return new BaseResponse<T>()
                {
                    Data = entity,
                    StatusCode = HttpStatusCode.OK,
                    Succeeded = true,
                    Message = string.IsNullOrWhiteSpace(entity as string) ? "Thành công" : entity as string,
                    Meta = meta
                };
            }
            return new BaseResponse<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = string.IsNullOrWhiteSpace(message) ? "Thành công" : message,
                Meta = meta
            };
        }

        public BaseResponse<T> Unauthorized<T>(string? message)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = "Lỗi xác thực"
            };
        }

        public BaseResponse<T> BadRequest<T>(string message, Dictionary<string, string[]>? errors = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "Yêu cầu thất bại" : message,
                Errors = errors ?? []
            };
        }

        public BaseResponse<T> Conflict<T>(string? message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.Conflict,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "Đã có xung đột xảy ra" : message
            };
        }

        public BaseResponse<T> UnprocessableEntity<T>(string? message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "Dữ liệu không hợp lệ" : message
            };
        }

        public BaseResponse<T> NotFound<T>(string? message = null)
        {
            return new BaseResponse<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = string.IsNullOrWhiteSpace(message) ? "Không tìm thấy dữ liệu" : message
            };
        }

        public BaseResponse<T> Created<T>(T entity, string? message = null, object? meta = null)
        {
            return new BaseResponse<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = message ?? "Đã tạo thành công",
                Meta = meta
            };
        }

        public PaginatedResult<T> Success<T>(List<T> data, long count, int page, int pageSize, string? message = null)
        {
            return new(true, data, message ?? "Thành công", count, page, pageSize, HttpStatusCode.OK);
        }
        public PaginatedResult<T> InternalServerError<T>(long count, int page, int pageSize, string? message = null)
        {
            return new(false, [], message ?? "Yêu cầu thất bại", count, page, pageSize, HttpStatusCode.InternalServerError);
        }
        public PaginatedResult<T> Unauthorized<T>(long count, int page, int pageSize, string? message = null)
        {
            return new(false, [], message ?? "Lỗi xác thực", count, page, pageSize, HttpStatusCode.Unauthorized);
        }
        public PaginatedResult<T> BadRequest<T>(long count, int page, int pageSize, string? message = null)
        {
            return new(false, [], message ?? "Yêu cầu thất bại", count, page, pageSize, HttpStatusCode.BadRequest);
        }
    }
}
