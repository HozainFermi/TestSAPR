using System.Reflection.Metadata.Ecma335;

namespace TestSAPR.Domain.Model
{
    public class Part
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; } = 0;
        public List<Part>? Children { get; set; }
    }
}
