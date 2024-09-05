namespace XrayUi.Application.Contracts.Clients;

public static class RemoveClient
{
    public readonly record struct Request(Guid Id);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success : Response;

        public sealed record NotFound : Response;
    }
}
