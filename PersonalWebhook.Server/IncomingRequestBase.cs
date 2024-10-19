namespace PersonalWebhook.Server
{
    public class IncomingRequestBase(string sessionId)
    {
        public string Method { get; set; } = string.Empty;
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public string SessionId { get; set; } = sessionId;
        public string RequestBody { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public List<object> Headers { get; set; } = [];
        public string AbsoluteUri { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public string RemoteIpAddress { get; set; } = string.Empty;
        public int RemotePort { get; set; }
    }
}
