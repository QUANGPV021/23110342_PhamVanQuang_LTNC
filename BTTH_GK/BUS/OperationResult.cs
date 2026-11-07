namespace BUS
{
    /// <summary>
    /// Đối tượng đóng gói kết quả xử lý nghiệp vụ để truyền từ tầng BUS lên tầng GUI.
    /// </summary>
    public class OperationResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public string? InvalidField { get; }

        private OperationResult(bool isSuccess, string message, string? invalidField = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            InvalidField = invalidField;
        }

        public static OperationResult Success(string message) =>
            new OperationResult(true, message);

        public static OperationResult Failure(string message, string? invalidField = null) =>
            new OperationResult(false, message, invalidField);
    }
}
