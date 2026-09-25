namespace MyWallet3.API.DTOs.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Fail(string error) => new Result { IsSuccess = false, Error = error };
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
        public new static Result<T> Fail(string error) => new Result<T> { IsSuccess = false, Error = error };
    }
}
