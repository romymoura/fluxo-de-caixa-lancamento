namespace FluxoDeCaixa.Lancamento.Unit.WebApi;

public static class TestHelpers
{
    public static System.Security.Claims.ClaimsPrincipal CreateClaimsPrincipal(string userId)
    {
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId)
        };
        return new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(claims));
    }
}