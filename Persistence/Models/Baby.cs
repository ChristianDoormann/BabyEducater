using System;

namespace Persistence.Models
{
    public class Baby
    {
        public Guid BabyId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}