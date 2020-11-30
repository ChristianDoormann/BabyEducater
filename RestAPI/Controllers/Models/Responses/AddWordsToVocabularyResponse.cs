using System.Collections.Generic;

namespace RestAPI.Controllers.Models.Responses
{
    public class AddWordsToVocabularyResponse
    {
        public int WordsAdded { get; set; }
        public IEnumerable<string> FamilyMembersMentioned { get; set; }
    }
}