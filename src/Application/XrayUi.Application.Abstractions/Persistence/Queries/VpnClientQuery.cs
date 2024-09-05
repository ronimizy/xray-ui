using SourceKit.Generators.Builder.Annotations;

namespace XrayUi.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record VpnClientQuery(Guid[] Ids);
