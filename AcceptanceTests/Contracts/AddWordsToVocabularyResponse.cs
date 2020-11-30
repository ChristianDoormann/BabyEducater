using System.Collections.Generic;

namespace AcceptanceTests.Contracts
{
    public class AddWordsToVocabularyResponse
    {
        public int WordsAdded { get; set; }
        public IEnumerable<string> FamilyMembersMentioned { get; set; }
    }
}