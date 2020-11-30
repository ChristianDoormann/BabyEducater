using System;
using System.Reflection;
using System.Text;
using AutoFixture.Kernel;

namespace Infrastructure.SpecimenBuilders
{
    public class TranscriptionSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Random _random;
        
        public TranscriptionSpecimenBuilder()
        {
            _random = new Random();
        }
        
        public object Create(object request, ISpecimenContext context)
        {
            var param = request as ParameterInfo;
            if (param == null)
                return new NoSpecimen();
            
            if (!param.Name!.ToLower().Equals("transcription"))
                return new NoSpecimen();
            
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < _random.Next(5000); i++)
            {
                stringBuilder
                    .Append(i + " ");
            }

            return stringBuilder.ToString();
        }
    }
}