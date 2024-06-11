using System.Net;
using Microsoft.EntityFrameworkCore;
using SMSProvider.Domain;
using SMSProvider.Repository.Context;
using SMSProvider.Service.Provider;
using SMSProvider.Service.Provider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Service.Tests;

public class SmsProviderServiceTest
{
    private readonly SmsProviderService _smsProviderService;
    private readonly SmsProviderDBContext _dbContext;

    public SmsProviderServiceTest()
    {
        var options = new DbContextOptionsBuilder<SmsProviderDBContext>()
            .UseInMemoryDatabase(databaseName: "SmsProviderDB")
            .Options;

        _dbContext = new SmsProviderDBContext(options);
        _smsProviderService = new SmsProviderService(_dbContext);
    }

    [Fact]
    public async Task AddProvider_ShouldReturnBadRequest_WhenProviderAlreadyExists()
    {
        // Arrange
        var existingProvider = new Domain.SmsProvider("ExistingProvider", 0.05m, ProviderStatus.Available);
        _dbContext.SmsProviders.Add(existingProvider);
        await _dbContext.SaveChangesAsync();

        var addProviderRequestModel = new ProviderRequestModel()
        {
            ProviderName = "ExistingProvider",
            CostPerSms = 0.10m
        };

        // Act
        var result = await _smsProviderService.AddProvider(addProviderRequestModel);

        // Assert
        Assert.Null(result.Data);
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        Assert.Equal(ResponseCode.Error, result.ResponseCode);
        Assert.Equal("Provider Already exists", result.Message);
    }

    [Fact]
    public async Task AddProvider_ShouldAddProviderSuccessfully()
    {
        // Arrange
        var addProviderRequestModel = new ProviderRequestModel
        {
            ProviderName = "NewProvider",
            CostPerSms = 0.10m
        };

        // Act
        var result = await _smsProviderService.AddProvider(addProviderRequestModel);

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal("NewProvider", result.Data.ProviderName);
        Assert.Equal(0.10m, result.Data.CostPerSms);

        var providerInDb = await _dbContext.SmsProviders.FirstOrDefaultAsync(p => p.ProviderName == "NewProvider");
        Assert.NotNull(providerInDb);
    }

    [Fact]
    public async Task GetProviders_ShouldReturnProviders()
    {
        // Arrange
        var provider1 = new Domain.SmsProvider("Provider1", 0.05m, ProviderStatus.Available);
        var provider2 = new Domain.SmsProvider("Provider2", 0.10m, ProviderStatus.Available);
        _dbContext.SmsProviders.AddRange(provider1, provider2);
        await _dbContext.SaveChangesAsync();

        var basePage = new BasePage { Page = 1, PageSize = 10 };

        // Act
        var result = await _smsProviderService.GetProviders(basePage);

        // Assert
        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(result.Data, p => p.ProviderName == "Provider1");
        Assert.Contains(result.Data, p => p.ProviderName == "Provider2");
    }
}