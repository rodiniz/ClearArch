using Wolverine.Http;

namespace SampleApp.Api;

public sealed record WorkItemCreationResponse(Guid Id)
    : CreationResponse($"/api/workitems/{Id}");
public sealed record WorkItemCreated(Guid Id);