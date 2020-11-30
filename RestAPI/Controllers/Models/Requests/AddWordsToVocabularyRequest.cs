using System.ComponentModel.DataAnnotations;

namespace RestAPI.Controllers.Models.Requests
{
    public class AddWordsToVocabularyRequest
    {
        [Required]
        public string Transcription { get; set; }
    }
}