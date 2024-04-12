namespace StationeryAPI.ShoppingModels
{
    public class UpdateDealer
    {
        public string UserId { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Fullname { get; set; }

        public string Passw { get; set; } = null!;


        public bool? Active { get; set; }
    }
}
