namespace f_backend_gestafe.Configurations
{
    public class PasswordResetSettings
    {
        public string FrontendUrl { get; set; } = "http://localhost:3000/redefinir-senha";
        public int ExpiresMinutes { get; set; } = 30;
    }
}
