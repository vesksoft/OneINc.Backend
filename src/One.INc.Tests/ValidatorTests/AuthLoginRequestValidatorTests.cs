using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Validators;

namespace One.INc.Tests.ValidatorTests
{
    public class AuthLoginRequestValidatorTests
    {

        [Theory]
        [InlineData("", "abc")]
        [InlineData("Hello", "")]
        [InlineData("Hello Hello", null)]
        [InlineData(null,"Hello Hello")]
        public void Test_ValidatorWhenRequestInCorrect_ResultInValidValidation(string userName, string passwrod)
        {
            // Arrange 
            var validator = new AuthLoginRequestValidator();
            AuthLoginRequest encodingRequest = new AuthLoginRequest
            {
                Password = passwrod,
                Username = userName
            };

            // Act 
            var result = validator.Validate(encodingRequest);

            // Assert 
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count() > 0);
        }

        [Theory]
        [InlineData("abc","abc")]
        [InlineData("Hello","asaa")]
        [InlineData("Hello Hello", "aqwrva")]
        public void Test_ValidatorWhenRequestCorrect_ResultValidValidation(string userName, string passwrod)
        {
            // Arrange 
            var validator = new AuthLoginRequestValidator();
            AuthLoginRequest encodingRequest = new AuthLoginRequest
            {
                Password = passwrod,
                Username = userName
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
