using PrimeCareMed.Core.Common;

namespace PrimeCareMed.Core.Entities
{
    public class Medicine : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
#nullable enable
        public ICollection<MedicinePrescription>? MedicinePrescriptions { get; set; } = new List<MedicinePrescription>();
#nullable disable
    }
}
