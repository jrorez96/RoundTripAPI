namespace RoundTripAPI
{
    public class RentalData
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Passport { get; set; }
        public string? ClientEmail { get; set; } // Correo del cliente
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? FechaRecogida { get; set; }
        public string? FechaEntrega { get; set; }
        public string? LugarRecogida { get; set; }
        public string? LugarEntrega { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
