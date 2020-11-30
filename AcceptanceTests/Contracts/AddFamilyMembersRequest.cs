using System.Collections.Generic;

namespace AcceptanceTests.Contracts
{
    public class AddFamilyMembersRequest
    {
        public IEnumerable<string> Names { get; set; }
    }
}