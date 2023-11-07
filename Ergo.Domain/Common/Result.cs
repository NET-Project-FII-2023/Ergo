namespace Ergo.Domain.Common
{
    public class Result<T> where T : class
    {
        private Result(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, null!);
        }
        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, null!, error);
        }
    }
}
