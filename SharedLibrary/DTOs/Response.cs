
namespace SharedLibrary.DTOs
{
    public class Response<T> where T : class
    {
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }
        public bool IsSuccessfull { get; private set; }
        public ErrorDto? Errors { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessfull = true };
        }
        public static Response<T> Fail(ErrorDto dto, int statusCode)
        {
            return new Response<T> { Errors = dto, StatusCode = statusCode, IsSuccessfull = false};
        }
        public static Response<T> Fail(ErrorDto dto, int statusCode, bool isShow = true)
        {
            return new Response<T> { Errors = dto, StatusCode = statusCode, IsSuccessfull = false };
        }
        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow = true)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> { Errors = errorDto, StatusCode = statusCode, IsSuccessfull = false };
        }
    }
}