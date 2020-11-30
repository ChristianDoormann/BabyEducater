using System;
using System.Collections.Generic;

namespace AcceptanceTests.Models
{
    public class Baby
    {
        public Guid BabyId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public IEnumerable<string> Vocabulary { get; set; }
        public IEnumerable<string> FamilyMembers { get; set; }
    }
}