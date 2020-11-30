using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Repositories;
using Baby = Persistence.Models.Baby;

namespace Shared.Services
{
    public interface IBabyService
    {
        Task<Guid> CreateBaby(string name, int age);
        Task DeleteBaby(Guid babyId);
        Task AddFamilyMembers(Guid babyId, IEnumerable<string> familyMemberNames);
        Task<(int wordsAdded, IEnumerable<string> familyMembersNamed)> IncreaseVocabulary(Guid babyId, string transcript);
    }
    
    public class BabyService : IBabyService
    {
        private readonly IBabyRepository _babyRepository;

        public BabyService(IBabyRepository babyRepository)
        {
            _babyRepository = babyRepository;
        }
        
        public async Task<Guid> CreateBaby(string name, int age)
        {
            var id = Guid.NewGuid();
            await _babyRepository.CreateBaby(new Baby
            {
                BabyId = id,
                Name = name,
                Age = age
            });
            return id;
        }

        public async Task DeleteBaby(Guid babyId)
        {
            await _babyRepository.DeleteBaby(babyId);
        }

        public async Task AddFamilyMembers(Guid babyId, IEnumerable<string> familyMemberNames)
        {
            if (familyMemberNames == null || !familyMemberNames.Any())
                return;

            await _babyRepository.AddFamilyMembers(babyId, familyMemberNames);
        }

        public async Task<(int wordsAdded, IEnumerable<string> familyMembersNamed)> IncreaseVocabulary(Guid babyId, string transcript)
        {
            var babyWords = transcript
                .Split(' ')
                .Where(word => !string.IsNullOrEmpty(word))
                .ToList();

            var uniqueWordsInserted = await _babyRepository.InsertIntoVocabulary(babyId, babyWords);
            
            var familyMemberNames = await _babyRepository.GetAllFamilyMemberNames(babyId) ?? new List<string>();
            var familyMembersMentioned = babyWords.Where(word => familyMemberNames.Contains(word));

            return (uniqueWordsInserted, familyMembersMentioned);
        }
    }
}