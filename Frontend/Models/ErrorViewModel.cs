namespace Frontend.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }
}
