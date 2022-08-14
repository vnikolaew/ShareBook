using ShareBook.Application.Identity;
using ShareBook.Application.Identity.Commands;
using ShareBook.Application.Identity.Commands.SignUp;
using ShareBook.Application.Identity.Queries.Authenticate;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Models.User.Factories;
using ShareBook.Domain.Models.User.Repositories;
using ShareBook.Infrastructure.Common.Security;

namespace ShareBook.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
	private readonly IJwtService _jwtService;
	private readonly IUserRepository _userRepository;
	private readonly IUserFactory _userFactory;
	private readonly IPasswordHasher _passwordHasher;

	public IdentityService(
		IJwtService jwtService,
		IUserRepository userRepository,
		IUserFactory userFactory,
		IPasswordHasher passwordHasher)
	{
		_jwtService = jwtService;
		_userRepository = userRepository;
		_userFactory = userFactory;
		_passwordHasher = passwordHasher;
	}
	public async Task<AuthenticationResult> SignUpAsync(SignUpRequestModel requestModel)
	{
		var existingUser = await _userRepository.FindByEmail(requestModel.Email);
		
		if (existingUser is not null)
		{
			return AuthenticationResult.Failure(new []{ "Email is already taken."});
		}

		var newUser = _userFactory
		              .WithEmail(requestModel.Email)
		              .WithUsername(requestModel.Username)
		              .WithDefaultProfile()
		              .WithPassword(_passwordHasher.Secure(requestModel.Password))
		              .Build();
		
		return GenerateAuthResultFor(newUser);
	}
	
	private AuthenticationResult GenerateAuthResultFor(User newUser)
	{
		var tokenForUser = _jwtService.GenerateTokenForUser(newUser);
		return AuthenticationResult.Success(newUser.Id, tokenForUser);
	}

	public async Task<AuthenticationResult> Authenticate(AuthRequestModel authRequestModel)
	{
		var existingUser = await _userRepository.FindByEmail(authRequestModel.Email);
		if (existingUser is null)
		{
			return AuthenticationResult.Failure(new []
			{
				"User with the provided e-mail does not exist."
			});
		}

		var passwordMatch = _passwordHasher.Verify(existingUser.Password, authRequestModel.Password);
		if (!passwordMatch)
		{
			return AuthenticationResult.Failure(new []
			{
				"Invalid credentials. Please try again."
			});
		}

		return GenerateAuthResultFor(existingUser);
	}

	public Task<User?> FindById(UserId userId, CancellationToken cancellationToken = default)
		=> _userRepository.FindAsync(userId, cancellationToken);
}