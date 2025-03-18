using AutoMapper;
using CSharpFunctionalExtensions;
using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Common.Enums;
using FluxoDeCaixa.Lancamento.Common.RequestResponse;
using FluxoDeCaixa.Lancamento.WebApi.Utils.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluxoDeCaixa.Lancamento.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CashRegisterController : BaseController
    {
        private readonly ILogger<CashRegisterController> _logger;
        private readonly ICashRegisterAppService _cashRegisterAppService;

        public CashRegisterController(
            ILogger<CashRegisterController> logger,
            ICashRegisterAppService cashRegisterAppService,
            IMapper mapper) : base(mapper)
        {
            _logger = logger;
            _cashRegisterAppService = cashRegisterAppService;
        }

#if RELEASE
        [Authorize(Policy = "OnlyStore")]
#endif
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRequestWithData<CashRegisterResponseRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CashRegisterRequestResponse request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inicio processamento para caixa registradora");

            // TODO: Fazer validation para validar request, utilizar fluent
            //if (!validationResult.IsValid)
            //return BadRequest(validationResult.Errors);

            
            var idStore = this.GetCurrentUserId();
            var cashRegisterMessageRequest = _mapper.Map<Application.RequestResponse.CashRegisterRequest>(request);
            cashRegisterMessageRequest.IdStore = new Guid(idStore);


            Func<Application.RequestResponse.CashRegisterResponse, Result<Application.RequestResponse.CashRegisterRepoRequest>> bindCashRegisterRequestToPersistence = 
                (sendCashRegisterResponse) => {
                var repoRequest = new Application.RequestResponse.CashRegisterRepoRequest
                {
                    IdStore = idStore,
                    IdMessage = sendCashRegisterResponse.IdMessage.ToString(),
                    CreateDate = DateTime.UtcNow,
                    Amount = request.Amount,
                    Price = request.Price,
                    CashRegisterType = request.CashRegisterType
                };
                return Result.Success(repoRequest);
            };


            var cashPersistenceRequest = await _cashRegisterAppService.SendCashRegisterAsync(cashRegisterMessageRequest, cancellationToken)
                .Bind(requestPersistence => bindCashRegisterRequestToPersistence(requestPersistence))
                .Check(requestPsistence => _cashRegisterAppService.SendCashRegisterPersistenceAsync(requestPsistence, cancellationToken));


            var result = _mapper.Map<CashRegisterResponseRequest>(cashPersistenceRequest);

            var message = request.CashRegisterType switch
            {
                CashRegisterType.Debit => "Débito registrado com sucesso!",
                CashRegisterType.Credit => "Crédito registrado com sucesso!",
                _ => "Registro sem identificação!"
            };

            _logger.LogInformation("Fim processamento para caixa registradora");
            return Ok(result, message);
        }
    }
}
