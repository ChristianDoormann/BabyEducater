using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using RestAPI.Controllers.Models.Requests;
using RestAPI.Controllers.Models.Responses;
using Shared.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RestAPI.Controllers
{
    [ApiController]
    [Produces("Application/Json")]
    [Route("v1/baby")]
    [SwaggerResponse(500, "Internal Server Error")]
    public class BabyController : ControllerBase
    {
        private readonly IBabyRepository _babyRepository;
        private readonly IBabyService _babyService;

        public BabyController(
            IBabyRepository babyRepository,
            IBabyService babyService)
        {
            _babyRepository = babyRepository;
            _babyService = babyService;
        }

        /// <summary>
        /// Creates a new baby with no vocabulary and no family members.
        /// </summary>
        /// <param name="request">Contains the name and age of the baby to create.</param>
        /// <returns>Id of the baby created</returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponse(200, "A Baby has been created.", typeof(CreateBabyResponse))]
        public async Task<IActionResult> CreateBaby(
            [Required, FromBody] CreateBabyRequest request)
        {
            var babyId = await _babyService.CreateBaby(request.Name, request.Age);
            
            return Ok((new CreateBabyResponse {BabyId = babyId}));
        }
        
        /// <summary>
        /// Gets a Baby by it's Id.
        /// </summary>
        /// <param name="babyId">The id of the baby.</param>
        /// <returns>The Baby</returns>
        [HttpGet]
        [Route("{babyId}")]
        [SwaggerResponse(200, "The Baby", typeof(GetBabyResponse))]
        [SwaggerResponse(404, "There is no baby corresponding to that id")]
        public async Task<IActionResult> GetBaby(
            [Required, FromRoute] Guid babyId)
        {
            var baby = await _babyRepository.GetBaby(babyId);
            if (baby == null)
                return NotFound();

            var vocabulary = await _babyRepository.GetVocabulary(babyId);
            var familyMembers = await _babyRepository.GetAllFamilyMemberNames(babyId);
            
            return Ok(new GetBabyResponse
            {
                BabyId = baby.BabyId,
                Name = baby.Name,
                Age = baby.Age,
                Vocabulary = vocabulary,
                FamilyMembers = familyMembers
            });
        }
        
        /// <summary>
        /// Adds new words to the baby's vocabulary, granted that the words does not already exist in the vocabulary.
        /// </summary>
        /// <param name="babyId">The id of the baby.</param>
        /// <param name="request">Contains the transcription of the baby's speech.</param>
        /// <returns>The number of words added to the baby's vocabulary, together with all the words in the current request that is present on the family list.</returns>
        [HttpPost]
        [Route("{babyId}/vocabulary")]
        [SwaggerResponse(200, "New words have been added to the vocabulary. See response for details.", typeof(AddWordsToVocabularyResponse))]
        [SwaggerResponse(404, "There is no baby corresponding to that id")]
        public async Task<IActionResult> AddWordsToVocabulary(
            [Required, FromRoute] Guid babyId,
            [Required, FromBody] AddWordsToVocabularyRequest request)
        {
            var baby = await _babyRepository.GetBaby(babyId);
            if (baby == null)
                return NotFound();

            if (string.IsNullOrEmpty(request.Transcription))
                return BadRequest("The transcription cannot be empty.");
            
            var (distinctWordsInserted, familyMentioned) = await _babyService.IncreaseVocabulary(babyId, request.Transcription);

            return Ok(new AddWordsToVocabularyResponse
            {
                WordsAdded = distinctWordsInserted,
                FamilyMembersMentioned = familyMentioned
            });
        }

        /// <summary>
        /// Deletes the Baby.
        /// </summary>
        /// <param name="babyId">The id of the baby.</param>
        [HttpDelete]
        [Route("{babyId}")]
        [SwaggerResponse(200, "The Baby has been deleted")]
        [SwaggerResponse(404, "There is no baby corresponding to that id")]
        public async Task<IActionResult> DeleteBaby(
            [Required, FromRoute] Guid babyId)
        {
            var baby = await _babyRepository.GetBaby(babyId);
            if (baby == null)
                return NotFound();

            await _babyService.DeleteBaby(babyId);
            
            return Ok();
        }

        /// <summary>
        /// Adds family members to the family member list.
        /// </summary>
        /// <param name="babyId">The id of the baby.</param>
        /// <param name="request">Contains the names to append to the family list.</param>
        [HttpPost]
        [Route("{babyId}/familymembers")]
        [SwaggerResponse(200, "The family list has been appended with more names")]
        [SwaggerResponse(404, "There is no baby corresponding to that id")]
        public async Task<IActionResult> AppendToFamilyList(
            [Required, FromRoute] Guid babyId,
            [Required, FromBody] AddFamilyMembersRequest request)
        {
            var baby = await _babyRepository.GetBaby(babyId);
            if (baby == null)
                return NotFound();
            
            await _babyService.AddFamilyMembers(babyId, request.Names);
            
            return Ok();
        }
    }
}