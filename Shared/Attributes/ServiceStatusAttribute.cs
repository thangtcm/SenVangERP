namespace Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ServiceStatusAttribute(int responseCode, string message, string? userMessage = null) : Attribute
    {
        public int ResponseCode { get; } = responseCode;
        public string? Message { get; } = message;
        public string? UserMessage { get; } = userMessage;
    }
}
