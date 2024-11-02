using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels;
using AutoMapper;
using Domain.Contracts.Abstracts.Shared;
using Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppConfiguration _configuration;
        private readonly ICurrentTime _currentTime;
        private readonly IPasswordHash _passwordHash;
        private readonly IEmailService _emailService;

        public PlayerService(IMapper mapper, IUnitOfWork unitOfWork, AppConfiguration configuration, ICurrentTime currentTime, IPasswordHash passwordHash, IEmailService emailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _currentTime = currentTime;
            _passwordHash = passwordHash;
            _emailService = emailService;
        }

        public async Task<Result<object>> CreateAccount(RegisterUserDTO req)
        {
            var emailExist = await _unitOfWork.playerRepository.CheckEmailExist(req.Email);

            if (emailExist)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "This email already exist, Do you want to verify?",
                    Data = null
                };
            }

            _passwordHash.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var token = _emailService.GenerateRandomNumber();

            var player = new Player
            {
                UserName = req.FullName,
                Email = req.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = token,
                IsDeleted = false
            };

            await _emailService.SendOtpMail(req.FullName, token, req.Email);
            await _unitOfWork.playerRepository.AddAsync(player);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Register successfully! Please check mail to verify."
            };
        }

        public async Task<Result<object>> SignInAccount(LoginUserDTO req)
        {
            var user = await _unitOfWork.playerRepository.GetPlayerByEmail(req.Email);

            if (user == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "User not found.",
                    Data = null
                };
            }

            var isPasswordValid = _passwordHash.VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Incorrect password.",
                    Data = null
                };
            }

            if (user.VerifiedAt == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Please verify your email.",
                    Data = null
                };
            }

            var token = user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());

            return new Result<object>
            {
                Error = 0,
                Message = $"Welcome back, {user.UserName}!",
                Data = token
            };
        }

        public async Task<Result<object>> Verify(VerifyTokenDTO request)
        {
            var verify = await _unitOfWork.playerRepository.Verify(request.Token);

            if (verify == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Invalid token"
                };
            }

            verify.VerifiedAt = DateTime.UtcNow;
            verify.VerificationToken = null;
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Verify successfully!"
            };
        }
    }
}
