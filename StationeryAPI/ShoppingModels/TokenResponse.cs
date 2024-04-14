namespace demo_jwt_netcore.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public int Role {  get; set; }

        public string id { get; set; }
    }
}
