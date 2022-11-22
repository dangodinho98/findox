using AutoMapper;
using Findox.Application.Dto.Account;
using Findox.Infra.Data.Repositories.User;

namespace Findox.Application.Services.Account;

using Findox.Domain.Entities;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

        var mapperConfiguration = new MapperConfiguration(c =>
        {
            c.CreateMap<CreateAccountRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt
                    => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.Roles, opt
                    => opt.MapFrom(src => src.RoleNames.Select(n => new Role(default, n))));

            c.CreateMap<UpdateAccountRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt
                    => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
                .ForMember(dest => dest.Roles, opt
                    => opt.MapFrom(src => src.RoleNames.Select(n => new Role(default, n))));
        });

        _mapper = new Mapper(mapperConfiguration);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _accountRepository.GetAllAsync();
    }

    public async Task<Account> GetByIdAsync(int id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }

    public async Task<Account?> CreateAsync(CreateAccountRequest request)
    {
        var accountRequest = _mapper.Map<Account>(request);
        return await _accountRepository.CreateAsync(accountRequest);
    }

    public async Task DeleteAsync(int id)
    {
        await _accountRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(int id, UpdateAccountRequest request)
    {
        var accountRequest = _mapper.Map<Account>(request);
        await _accountRepository.UpdateAsync(id, accountRequest);
    }
}