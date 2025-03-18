using AutoMapper;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Application.Services;
using FluxoDeCaixa.Lancamento.Common.Enums;
using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using FluxoDeCaixa.Lancamento.Domain.Services;
using FluxoDeCaixa.Lancamento.Unit.Application.SourceData;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;



namespace FluxoDeCaixa.Lancamento.Unit.Application;

public class CashRegisterAppServiceTests
{
    private readonly Mock<ISqsPublisher> _sqsPublisherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICasheRegisterRepository> _casheRegisterRepositoryMock;
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly Mock<ILogger<CashRegisterAppService>> _loggerMock;
    private readonly CashRegisterAppService _service;

    public CashRegisterAppServiceTests()
    {
        _sqsPublisherMock = new Mock<ISqsPublisher>();
        _mapperMock = new Mock<IMapper>();
        _casheRegisterRepositoryMock = new Mock<ICasheRegisterRepository>();
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _loggerMock = new Mock<ILogger<CashRegisterAppService>>();

        _service = new CashRegisterAppService(
            _sqsPublisherMock.Object,
            _mapperMock.Object,
            _casheRegisterRepositoryMock.Object,
            _storeRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Send mensage sqs success")]
    public async Task SendCashRegisterMessageAsync_ShouldReturnSuccess_WhenMessageIsSent()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestFaker.Generate();

        var store = new Store { Id = request.StoreId.Value };
        var response = new CashRegisterResponse { MessageId = Guid.NewGuid() };

        _storeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(request.StoreId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(store);

        _sqsPublisherMock
            .Setup(publisher => publisher.PublishMessageAsync(It.IsAny<string>(), null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Amazon.SQS.Model.SendMessageResponse { MessageId = response.MessageId.ToString() });

        _mapperMock
            .Setup(mapper => mapper.Map<CashRegisterResponse>(It.IsAny<object>()))
            .Returns(response);

        // Act
        var result = await _service.SendCashRegisterMessageAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.MessageId.Should().Be(response.MessageId);
    }

    [Fact(DisplayName = "Persistence success")]
    public async Task SendCashRegisterPersistenceAsync_ShouldReturnSuccess_WhenPersistenceIsSuccessful()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestPersistenceFaker.Generate();
        var store = new Store { Id = Guid.Parse(request.StoreId) };
        var product = new Product { StoreId = store.Id };
        var response = new CashRegisterRepoResponse { ProductId = Guid.NewGuid() };

        _storeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(store.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(store);

        _casheRegisterRepositoryMock
            .Setup(repo => repo.CreateAsync<Guid>(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mapperMock
            .Setup(mapper => mapper.Map<CashRegisterRepoResponse>(It.IsAny<object>()))
            .Returns(response);

        _mapperMock
            .Setup(mapper => mapper.Map<Product>(It.IsAny<object>()))
            .Returns(product);

        

        // Act
        var result = await _service.SendCashRegisterPersistenceAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ProductId.Should().Be(response.ProductId);
    }

    [Fact(DisplayName = "Persistence fail")]
    public async Task SendCashRegisterPersistenceAsync_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestPersistenceFaker.Generate();

        var store = new Store { Id = Guid.Parse(request.StoreId) };
        var product = new Product { StoreId = store.Id };
        var response = new CashRegisterRepoResponse { ProductId = Guid.NewGuid() };

        _casheRegisterRepositoryMock
            .Setup(repo => repo.CreateAsync<Guid>(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mapperMock
            .Setup(mapper => mapper.Map<CashRegisterRepoResponse>(It.IsAny<object>()))
            .Returns(response);

        _mapperMock
            .Setup(mapper => mapper.Map<Product>(It.IsAny<object>()))
            .Returns(product);

        _storeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(Guid.Parse(request.StoreId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro simulado"));

        // Act
        var result = await _service.SendCashRegisterPersistenceAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Erro simulado");
    }


    [Fact(DisplayName = "Store exists is true")]
    public async Task SendCashRegisterMessageAsync_ShouldReturnSuccess_WhenStoreExists()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestFaker.Generate();
        var store = new Store { Id = request.StoreId.Value };
        var response = new CashRegisterResponse { MessageId = Guid.NewGuid() };

        _storeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(request.StoreId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(store);

        _sqsPublisherMock
            .Setup(publisher => publisher.PublishMessageAsync(It.IsAny<string>(), null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Amazon.SQS.Model.SendMessageResponse { MessageId = response.MessageId.ToString() });

        _mapperMock
            .Setup(mapper => mapper.Map<CashRegisterResponse>(It.IsAny<object>()))
            .Returns(response);

        // Act
        var result = await _service.SendCashRegisterMessageAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.MessageId.Should().Be(response.MessageId);
    }

    [Fact(DisplayName = "Store exists is false")]
    public async Task SendCashRegisterMessageAsync_ShouldReturnFailure_WhenStoreDoesNotExist()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestFaker.Generate();

        _storeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(request.StoreId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store)null);

        // Act
        var result = await _service.SendCashRegisterMessageAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Loja não encontrada em nosso sistema");
    }

    [Fact(DisplayName = "id Store null attempt fail")]
    public async Task SendCashRegisterMessageAsync_ShouldReturnFailure_WhenStoreIdIsNull()
    {
        // Arrange
        var request = CashRegisterAppServiceSourceData.CashRegisterRequestFaker.Generate();
        request.StoreId = null;

        // Act
        var result = await _service.SendCashRegisterMessageAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Identificador da loja não é válido!");
    }

}
