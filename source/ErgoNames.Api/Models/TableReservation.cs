namespace ErgoNames.Api.Models
{
    public class TableReservation
    {
        public DateTime Requested { get; set; }

        public string? Name { get; set; }

        public TableReservation()
        {
            Requested = DateTime.UtcNow;
        }

        public TableReservation(string name) : this()
        {
            Name = name.ToLowerInvariant();
        }
    }
}
