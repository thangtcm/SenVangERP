using Shared.Attributes;

namespace Shared.Enums
{
    public enum ResponseCodes
    {
        [ServiceStatus(200, "Successful")]
        SUCCESS = 0,

        [ServiceStatus(400, "Bad Request")]
        BAD_REQUEST = 1,

        [ServiceStatus(404, "Not Found")]
        NOT_FOUND = 2,

        [ServiceStatus(401, "Authorization Error")]
        UNAUTHORIZED = 3,

        [ServiceStatus(500, "Exception")]
        EXCEPTION = 4
    }
}
