namespace XrayUi.Application.Contracts.Clients;

public static class GetClientConfig
{
    public readonly record struct Request(Guid ClientId);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(string Config) : Response;

        public sealed record NotFound : Response;
    }
}
