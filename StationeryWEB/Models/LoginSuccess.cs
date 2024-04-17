namespace StationeryWEB.Models
{
    public class LoginSuccess
    {
        public string id { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken {  get; set; }
        public int role {  get; set; }
    }
}
