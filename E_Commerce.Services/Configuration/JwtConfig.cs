namespace E_Commerce.Services.Configuration
{
    public class JwtConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string DurationInDays { get; set; }
         

    }
}
