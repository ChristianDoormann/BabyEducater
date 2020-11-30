using RestAPI.Controllers.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class AddWordsToVocabularyResponseExample : IExamplesProvider<AddWordsToVocabularyResponse>
    {
        public AddWordsToVocabularyResponse GetExamples()
        {
            return new AddWordsToVocabularyResponse
            {
                WordsAdded = 5,
                FamilyMembersMentioned = new [] {"Kaj"}
            };
        }
    }
}