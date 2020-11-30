using RestAPI.Controllers.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestAPI.Swagger.ModelExamples
{
    public class AddWordsToVocabularyRequestExample : IExamplesProvider<AddWordsToVocabularyRequest>
    {
        public AddWordsToVocabularyRequest GetExamples()
        {
            return new AddWordsToVocabularyRequest
            {
                Transcription = "Hello my name is Kaj"
            };
        }
    }
}