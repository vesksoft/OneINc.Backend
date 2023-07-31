using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Validators;

namespace One.INc.Tests.ValidatorTests
{
    public class EncodingRequestValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Test_ValidatorWhenRequestContentBad_ResultValidationFailure(string contentValue)
        {
            // Arrange 
            var validator = new EncodingRequestValidator();
            EncodingRequest encodingRequest = new EncodingRequest 
            {
                Content = contentValue,
            };

            // Act 
            var result = validator.Validate(encodingRequest);

            // Assert 
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count() > 0);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("Hello")]
        [InlineData("Hello Hello")]
        public void Test_ValidatorWhenRequestCorrect_ResultValidValidation(string contentValue)
        {
            // Arrange 
            var validator = new EncodingRequestValidator();
            EncodingRequest encodingRequest = new EncodingRequest
            {
                Content = contentValue,
            };

            // Act 
            var result = validator.Validate(encodingRequest);

            // Assert 
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count() == 0);
        }
    }
}
