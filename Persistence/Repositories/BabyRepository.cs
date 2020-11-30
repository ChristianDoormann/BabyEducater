using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface IBabyRepository
    {
        Task CreateBaby(Baby baby);
        Task<Baby> GetBaby(Guid babyId);
        Task AddFamilyMembers(Guid babyId, IEnumerable<string> familyMemberNames);
        Task<IEnumerable<string>> GetAllFamilyMemberNames(Guid babyId);
        Task<int> InsertIntoVocabulary(Guid babyId, List<string> babyWords);
        Task<IEnumerable<string>> GetVocabulary(Guid babyId);
        Task DeleteBaby(Guid babyId);
    }
    
    public class BabyRepository : IBabyRepository
    {
        private const int DBMaxRowsToInsert = 1000;
        private readonly string _connectionString;

        public BabyRepository(IPersistenceConfiguration configuration)
        {
            _connectionString = configuration.ConnectionString;
        }
        
        private string CreateBabyStatement(Baby baby) => 
            $"Insert Into Babies Values ('{baby.BabyId}', '{baby.Name}', {baby.Age})";
        private string DeleteBabyStatement(Guid babyId) =>
            $"Delete From Babies Where BabyId = '{babyId}';" +
            $"Delete From Vocabulary Where BabyId = '{babyId}';" +
            $"Delete From FamilyMembers Where BabyId = '{babyId}';";
        private string GetBabyStatement(Guid babyId) => 
            $"Select * From Babies Where BabyId = '{babyId}'";
        private string GetAllFamilyMemberNamesStatement(Guid babyId) => 
            $"Select Name From FamilyMembers Where BabyId = '{babyId}'";
        private string GetVocabularyStatement(Guid babyId) => 
            $"Select Word From Vocabulary Where BabyId = '{babyId}'";
        
        private string AddFamilyMembersStatement(Guid babyId, string[] familyMemberNames)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Insert Into FamilyMembers Values ");
            for (var i = 0; i < familyMemberNames.Length; i++)
            {
                stringBuilder.Append($"('{babyId}', '{familyMemberNames[i]}')");
                if (i < familyMemberNames.Length - 1)
                    stringBuilder.Append(", ");
            }

            return stringBuilder.ToString();
        }

        private string InsertWordsIntoVocabularyStatement(Guid babyId, string[] babyWords)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Insert Into Vocabulary Values ");
            for (var i = 0; i < babyWords.Length; i++)
            {
                stringBuilder.Append($"('{babyId}', '{babyWords[i]}')");
                if (i < babyWords.Length - 1)
                    stringBuilder.Append(", ");
            }

            return stringBuilder.ToString();
        }
        
        public async Task CreateBaby(Baby baby)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(CreateBabyStatement(baby));
            }
        }

        public async Task<Baby> GetBaby(Guid babyId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<Baby>(GetBabyStatement(babyId));
                return result.SingleOrDefault();
            }
        }

        public async Task AddFamilyMembers(Guid babyId, IEnumerable<string> familyMemberNames)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(AddFamilyMembersStatement(babyId, familyMemberNames.ToArray()));
            }
        }

        public async Task<IEnumerable<string>> GetAllFamilyMemberNames(Guid babyId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<string>(GetAllFamilyMemberNamesStatement(babyId));
            }
        }

        public async Task<int> InsertIntoVocabulary(Guid babyId, List<string> babyWords)
        {
            if (babyWords == null || babyWords.LongCount() == 0)
                return 0;

            var distinctWordsInserted = 0;
            var wordsProcessed = 0;
            do
            {
                var remainingWords = babyWords.Count - wordsProcessed;
                var badgeSize = remainingWords > DBMaxRowsToInsert ? DBMaxRowsToInsert : remainingWords;
                var badge = babyWords.GetRange(wordsProcessed, badgeSize);
                var result = await InsertWordsIntoVocabulary(babyId, badge.ToArray());
                distinctWordsInserted += result;
                wordsProcessed += badge.Count;
            } while (wordsProcessed < babyWords.Count);
            
            return distinctWordsInserted;
        }
        
        private async Task<int> InsertWordsIntoVocabulary(Guid babyId, string[] words)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteAsync(InsertWordsIntoVocabularyStatement(babyId, words));
            }
        }

        public async Task<IEnumerable<string>> GetVocabulary(Guid babyId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<string>(GetVocabularyStatement(babyId));
            }
        }

        public async Task DeleteBaby(Guid babyId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(DeleteBabyStatement(babyId));
            }
        }
    }
}