using AutoMapper;
using CSharpFunctionalExtensions;
using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Common.Enums;
using FluxoDeCaixa.Lancamento.Common.RequestResponse;
using FluxoDeCaixa.Lancamento.Unit.WebApi.SourceData;
using FluxoDeCaixa.Lancamento.WebApi.Controllers;
using FluxoDeCaixa.Lancamento.WebApi.Utils.RequestResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FluxoDeCaixa.Lancamento.Unit.WebApi;

public class CashRegisterControllerTests
{
    private readonly Mock<ILogger<CashRegisterController>> _loggerMock;
    private readonly Mock<ICashRegisterAppService> _cashRegisterAppServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CashRegisterController _controller;

    public CashRegisterControllerTests()
    {
        _loggerMock = new Mock<ILogger<CashRegisterController>>();
        _cashRegisterAppServiceMock = new Mock<ICashRegisterAppService>();
        _mapperMock = new Mock<IMapper>();

        // Configura o controlador com as dependências simuladas
        _controller = new CashRegisterController(
            _loggerMock.Object,
            _cashRegisterAppServiceMock.Object,
            _mapperMock.Object
        );

        // Configura o contexto HTTP para simular o usuário autenticado
        var httpContext = new DefaultHttpContext();
        httpContext.User = TestHelpers.CreateClaimsPrincipal("862e08eb-fa5b-4393-9e82-983002538378"); // Simula um usuário autenticado
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }


    [Fact(DisplayName = "Cash register credit success")]
    public async Task Post_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var arrange = CashRegisterControllerSourceData.ArrangeCasheRegisterCredit();

        // Configura os mocks
        _mapperMock.Setup(m => m.Map<CashRegisterRequest>(arrange.request))
            .Returns(arrange.cashRegisterMessageRequest);

        _cashRegisterAppServiceMock
            .Setup(s => s.SendCashRegisterMessageAsync(arrange.cashRegisterMessageRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(arrange.cashRegisterResponse));

        _cashRegisterAppServiceMock
            .Setup(s => s.SendCashRegisterPersistenceAsync(It.IsAny<CashRegisterRepoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(arrange.cashRegisterRepoResponse));

        _mapperMock.Setup(m => m.Map<CashRegisterResponseRequest>(It.IsAny<CashRegisterRepoRequest>()))
            .Returns(arrange.cashRegisterResponseRequest);

        // Act
        var result = await _controller.Post(arrange.request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseRequestWithData<CashRegisterResponseRequest>>(okResult.Value);
        Assert.Equal("Crédito registrado com sucesso!", response.Message);
        Assert.Equal(arrange.cashRegisterResponseRequest.MessageId, response.Data.MessageId);
        Assert.Equal(arrange.cashRegisterResponseRequest.ProductId, response.Data.ProductId);
    }

    [Fact(DisplayName = "Cash register credit fail")]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var arrange = CashRegisterControllerSourceData.ArrangeCasheRegisterCredit();

        // Configura o mock para retornar uma falha
        _mapperMock.Setup(m => m.Map<CashRegisterRequest>(arrange.request))
            .Returns(arrange.cashRegisterMessageRequest);

        _cashRegisterAppServiceMock
            .Setup(s => s.SendCashRegisterMessageAsync(It.IsAny<CashRegisterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<CashRegisterResponse>("Erro ao processar a requisição."));

        _cashRegisterAppServiceMock
            .Setup(s => s.SendCashRegisterPersistenceAsync(It.IsAny<CashRegisterRepoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(arrange.cashRegisterRepoResponse));

        _mapperMock.Setup(m => m.Map<CashRegisterResponseRequest>(It.IsAny<CashRegisterRepoRequest>()))
            .Returns(arrange.cashRegisterResponseRequest);

        // Act
        var result = await _controller.Post(arrange.request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseRequest>(badRequestResult.Value);
        Assert.Equal("Erro ao processar a requisição.", response.Message);
    }
}
