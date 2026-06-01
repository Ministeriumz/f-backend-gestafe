namespace f_backend_gestafe.Middleware.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<string> errors)
            : base("Dados inválidos.")
        {
            Errors = errors.ToArray();
        }

        public IReadOnlyCollection<string> Errors { get; }
    }
}
