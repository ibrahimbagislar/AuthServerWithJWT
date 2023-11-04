
namespace SharedLibrary.DTOs
{
    public class ErrorDto
    {
        public List<string> Errors { get; private set; } = new List<string>();
        public bool IsShow { get; private set; }
        public ErrorDto(string error,bool isShow=true)
        {
            Errors.Add(error);
            IsShow = isShow;
        }
        public ErrorDto(List<string> errors, bool isShow= true)
        {
            Errors = errors;
            IsShow = isShow;    
        }
    }
}
