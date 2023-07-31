namespace OneINc.Web.Common.Models.Responses
{
    public class AuthLoginResponse
    {
        public bool Success { get; }
        public Guid SessionId { get; }
        public AuthLoginResponse(bool success = false) 
        {
            this.Success = success;
            this.SessionId = Guid.NewGuid();
        }
    }
}
