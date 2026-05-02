namespace TestSAPR.Domain.Model
{
    public class Part
    {
        public required string Name { get; set; }
        public int Quantity { get; set; } = 1;
        public List<Part>? Children { get; set; }
    }
}
