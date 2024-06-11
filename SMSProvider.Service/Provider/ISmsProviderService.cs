using SMSProvider.Service.Provider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Service.Provider;

public interface ISmsProviderService
{
    Task<FuncResponseWithValue<GetProviderResponseModel>> AddProvider(ProviderRequestModel providerRequestModel);
    Task<PaginatedOutPut<GetProviderResponseModel>> GetProviders(BasePage basePage);
    Task<FuncResponseWithValue<GetProviderResponseModel>> UpdateProvider(int id, ProviderRequestModel providerRequestModel);
}