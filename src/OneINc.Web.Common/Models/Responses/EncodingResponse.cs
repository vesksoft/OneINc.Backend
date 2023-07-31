namespace OneINc.Web.Common.Models.Responses
{
    public class EncodingResponse
    {
        public bool Status { get; }

        public EncodingResponse(bool status) 
        {
            this.Status = status;
        }
    }
}
