namespace f_backend_gestafe.Middleware.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string field, string message)
            : base(message)
        {
            Field = field;
        }

        public string Field { get; }
    }
}
