using Microsoft.EntityFrameworkCore;
using Moq;
using SMSProvider.Domain;
using SMSProvider.Repository.Context;
using SMSProvider.Service.Sms;
using SMSProvider.Service.SmsProvider.Models;
using SMSProvider.Service.SmsProviderStrategy;

namespace SMSProvider.Service.Tests;

public class SmsServiceTest
{
    private readonly SmsProviderDBContext _dbContext;
    private readonly Mock<ISmsProvider> _smsProviderMock;
    private readonly SmsService _smsService;

    public SmsServiceTest()
    {
        var options = new DbContextOptionsBuilder<SmsProviderDBContext>()
            .UseInMemoryDatabase(databaseName: "SmsProviderDB")
            .Options;

        _dbContext = new SmsProviderDBContext(options);
        _smsProviderMock = new Mock<ISmsProvider>();

        List<ISmsProvider> smsProviders = new() { _smsProviderMock.Object };
        _smsService = new SmsService(_dbContext, smsProviders);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldSendSmsUsingSelectedProvider()
    {
        // Arrange
        var smsRequestModel = new SendSmsRequestModel
        {
            RecipientPhone = "+1234567890",
            MessageBody = "Hello, world!"
        };

        _smsProviderMock.Setup(p => p.SendSmsAsync(smsRequestModel.RecipientPhone, smsRequestModel.MessageBody))
            .ReturnsAsync(true);
        _smsProviderMock.Setup(p => p.GetProviderName()).Returns("MockProvider");

        var smsProvider = new Domain.SmsProvider
            ("MockProvider", 0.05m, ProviderStatus.Available);

        _dbContext.SmsProviders.Add(smsProvider);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _smsService.SendSmsAsync(smsRequestModel);

        // Assert
        Assert.True(result.Data);
        _smsProviderMock.Verify(p => p.SendSmsAsync(smsRequestModel.RecipientPhone, smsRequestModel.MessageBody), Times.Once);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldLogSmsAttempt()
    {
        // Arrange
        var smsRequestModel = new SendSmsRequestModel
        {
            RecipientPhone = "+1234567890",
            MessageBody = "Hello, world!"
        };

        _smsProviderMock.Setup(p => p.SendSmsAsync(smsRequestModel.RecipientPhone, smsRequestModel.MessageBody))
            .ReturnsAsync(true);
        _smsProviderMock.Setup(p => p.GetProviderName()).Returns("MockProvider");

        var smsProvider = new Domain.SmsProvider
        (
            "MockProvider",
            0.05m, ProviderStatus.Available
        );

        _dbContext.SmsProviders.Add(smsProvider);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _smsService.SendSmsAsync(smsRequestModel);

        // Assert
        Assert.True(result.Data);
        var log = await _dbContext.SmsLogs.FirstOrDefaultAsync();
        Assert.NotNull(log);
        Assert.Equal(smsRequestModel.RecipientPhone, log.RecipientPhone);
        Assert.Equal(smsRequestModel.MessageBody, log.MessageContent);
        Assert.Equal(MessageStatus.Sent, log.Status);
        Assert.Equal("MockProvider", log.ProviderUsed);
    }
}