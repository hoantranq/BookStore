namespace BookStore.API.ApplicationSettings
{
    /// <summary>
    /// This class is uses to read data from out previously created JWT section of appsettings.json 
    /// using IOptions feature of ASP.NET CORE
    /// </summary>
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
