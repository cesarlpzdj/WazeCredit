public class CustomMiddleware
{
    private readonly RequestDelegate _nextRequest;

    public CustomMiddleware(RequestDelegate nextRequest)
    {
        _nextRequest = nextRequest;
    }

    public async Task InvokeAsync(HttpContext context, TransientService transientService,
        ScopedService scopedService, SingletonService singletonService)
    {
        context.Items.Add("CustomMiddlewareTransient", "Transient Middlewarwe - " + transientService.GetGuid());
        context.Items.Add("CustomMiddlewareScoped", "Scoped Middlewarwe - " + scopedService.GetGuid());
        context.Items.Add("CustomMiddlewareSingleton", "Singleton Middlewarwe - " + singletonService.GetGuid());

        await _nextRequest(context);
    }
}