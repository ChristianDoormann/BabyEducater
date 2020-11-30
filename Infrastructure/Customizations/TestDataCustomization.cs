using AutoFixture;
using Infrastructure.SpecimenBuilders;

namespace Infrastructure.Customizations
{
    public class TestDataCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new TranscriptionSpecimenBuilder());
        }
    }
}