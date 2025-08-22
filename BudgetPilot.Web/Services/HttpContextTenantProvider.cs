using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BudgetPilot.Web.Services;

public class HttpContextTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid FamilyId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            var fromClaim = context?.User?.FindFirst("family_id")?.Value
                           ?? context?.User?.FindFirst("FamilyId")?.Value;
            var fromHeader = context?.Request?.Headers["X-Family-Id"].FirstOrDefault();
            var raw = fromClaim ?? fromHeader;
            return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
        }
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}


