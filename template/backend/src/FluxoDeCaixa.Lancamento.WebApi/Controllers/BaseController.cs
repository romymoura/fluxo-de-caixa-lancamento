using AutoMapper;
using FluxoDeCaixa.Lancamento.Common.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FluxoDeCaixa.Lancamento.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    public readonly IMapper _mapper;
    public BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }
    protected string GetCurrentUserId()
    {
        var result = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

#if DEBUG
        // Utilizado para loja fake, pois esse valor será recuperado do token via claim
        return string.IsNullOrEmpty(result) ? "862e08eb-fa5b-4393-9e82-983002538378" : result;
#else
        return result ?? string.Empty;
#endif
    }

    protected string GetCurrentUserEmail() => User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    protected IActionResult Ok<T>(T data) => base.Ok(new ResponseRequestWithData<T> { Data = data, Success = true });
    protected IActionResult Ok<T>(T data, string message) => base.Ok(new ResponseRequestWithData<T> { Data = data, Success = true, Message = message });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ResponseRequestWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ResponseRequest { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ResponseRequest { Message = message, Success = false });
}
