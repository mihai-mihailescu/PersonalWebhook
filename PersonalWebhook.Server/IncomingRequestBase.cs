namespace PersonalWebhook.Server
{
    public class IncomingRequestBase
    {
        public string Method { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string RequestBody {  get; set; } = string.Empty;
    }
}
