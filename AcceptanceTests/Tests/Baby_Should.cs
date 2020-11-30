using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AcceptanceTests.Helpers;
using AcceptanceTests.Infrastructure;
using AcceptanceTests.Models;
using Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Shared.Services;
using Shouldly;
using Xunit;

namespace AcceptanceTests.Tests
{
    [Collection(nameof(TestCollection))]
    public class Baby_Should
    {
        private readonly ApiHelper _apiHelper;

        public Baby_Should(TestFixtureData testFixtureData)
        {
            _apiHelper = testFixtureData.ServiceProvider.GetService<ApiHelper>();
        }

        [Theory, CustomAutoData]
        public async void Be_Created_With_Default_Values(Baby baby)
        {
            // Act
            var (createdStatusCode, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Assert
            createdStatusCode.ShouldBe(HttpStatusCode.OK);
            var (queryStatusCode, babyResponse) = await _apiHelper.QueryBaby(createBabyResponse.BabyId);
            queryStatusCode.ShouldBe(HttpStatusCode.OK);
            babyResponse.Name.ShouldBe(baby.Name);
            babyResponse.Age.ShouldBe(baby.Age);
            babyResponse.Vocabulary.ShouldBeEmpty();
            babyResponse.FamilyMembers.ShouldBeEmpty();
        }
        
        [Theory, CustomAutoData]
        public async void Be_Deleted_When_Calling_Delete(Baby baby)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Act
            var deleteStatusCode = await _apiHelper.DeleteBaby(createBabyResponse.BabyId);

            // Assert
            deleteStatusCode.ShouldBe(HttpStatusCode.OK);
            var (queryStatusCode, _) = await _apiHelper.QueryBaby(createBabyResponse.BabyId);
            queryStatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory, CustomAutoData]
        public async void Have_FamilyMembers(Baby baby, IEnumerable<string> familyMembers)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Act
            var responseStatusCode = await _apiHelper.AddFamilyMembers(createBabyResponse.BabyId, familyMembers);

            // Assert
            responseStatusCode.ShouldBe(HttpStatusCode.OK);
            var (statusCode, getBabyResponse) = await _apiHelper.QueryBaby(createBabyResponse.BabyId);
            statusCode.ShouldBe(HttpStatusCode.OK);
            getBabyResponse.FamilyMembers.Count().ShouldBe(familyMembers.Count());
        }
        
        [Theory, CustomAutoData]
        public async void Add_Words_To_Vocabulary(Baby baby, string transcription)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Act
            var (addWordsStatusCode, _) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            var (statusCode, getBabyResponse) = await _apiHelper.QueryBaby(createBabyResponse.BabyId);
            statusCode.ShouldBe(HttpStatusCode.OK);
            getBabyResponse.Vocabulary.Count().ShouldBe(transcription
                .Split(' ')
                .Count(word => !string.IsNullOrEmpty(word)));
        }
        
        [Theory, CustomAutoData]
        public async void Return_Distinct_Words_Count_Equal_To_Total_Word_Count_When_First_Addition_To_Vocabulary(Baby baby, string transcription)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Act
            var (addWordsStatusCode, addWordsToVocabularyResponse) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            addWordsToVocabularyResponse.WordsAdded.ShouldBe(transcription
                .Split(' ')
                .Count(word => !string.IsNullOrEmpty(word)));
        }
        
        [Theory, CustomAutoData]
        public async void Return_Distinct_Words_Count_Equal_To_Zero_When_First_Addition_To_Vocabulary_Repeated(Baby baby, string transcription)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);
            await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Act
            var (addWordsStatusCode, addWordsToVocabularyResponse) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            addWordsToVocabularyResponse.WordsAdded.ShouldBe(0);
        }
        
        [Theory, CustomAutoData]
        public async void Return_Distinct_Words_Count_Equal_To_One_When_First_Addition_To_Vocabulary_Repeated_Plus_One(Baby baby, string transcription)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);
            await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Act
            var (addWordsStatusCode, addWordsToVocabularyResponse) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription + " Hello");

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            addWordsToVocabularyResponse.WordsAdded.ShouldBe(1);
        }
        
        [Theory, CustomAutoData]
        public async void Return_Distinct_Words_Count_Equal_To_20000_When_First_Addition_To_Vocabulary_Contains_20000_Words(Baby baby)
        {
            // Arrange
            var transcription = CreateTranscriptionWithXNumberOfWords(20000);
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);

            // Act
            var (addWordsStatusCode, addWordsToVocabularyResponse) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription);

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            addWordsToVocabularyResponse.WordsAdded.ShouldBe(20000);
        }
        
        [Theory, CustomAutoData]
        public async void Return_FamilyMember_When_Adding_FamilyMember_To_Vocabulary(Baby baby, string transcription, string[] familyMembers)
        {
            // Arrange
            var (_, createBabyResponse) = await _apiHelper.CreateBaby(baby.Name, baby.Age);
            await _apiHelper.AddFamilyMembers(createBabyResponse.BabyId, familyMembers);

            // Act
            var (addWordsStatusCode, addWordsToVocabularyResponse) = await _apiHelper.AddWordsToVocabulary(createBabyResponse.BabyId, transcription + " " + familyMembers[0]);

            // Assert
            addWordsStatusCode.ShouldBe(HttpStatusCode.OK);
            addWordsToVocabularyResponse.FamilyMembersMentioned.ShouldContain(familyMembers[0]);
        }

        private string CreateTranscriptionWithXNumberOfWords(int wordCount)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < wordCount; i++)
            {
                stringBuilder.Append(i + " ");
            }

            return stringBuilder.ToString();
        }
    }
}