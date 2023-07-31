namespace One.INc.Tests.IntegrationTests
{
    public class CustomHeader
    {
        public string HeaderName { get;}
        public string HeaderValue { get;}

        public CustomHeader(string name, string value) 
        {
            HeaderName = name;
            HeaderValue = value;
        }
    }
}
