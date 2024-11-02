using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Contracts.Abstract.Account;
using Domain.Contracts.Abstracts.Shared;
using KitchenChaos.Validations;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using WebAPI.Controllers;

namespace KitchenChaos.Controllers
{
    public class PlayerController : BaseController
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerService playerService, IMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequest request)
        {
            var validator = new RegisterUserValidator();
            var validatorResult = validator.Validate(request);
            if (validatorResult.IsValid == false)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Missing value!",
                    Data = validatorResult.Errors.Select(x => x.ErrorMessage),
                });
            }

            var result = await _playerService.CreateAccount(_mapper.Map<RegisterUserDTO>(request));
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Login([FromForm] LoginUserRequest req)
        {
            var validator = new LoginUserValidator();
            var validationResult = validator.Validate(req);
            if (!validationResult.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Validation failed!",
                    Data = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var loginDto = _mapper.Map<LoginUserDTO>(req);
            var result = await _playerService.SignInAccount(loginDto);

            return Ok(result);
        }

        [HttpPost("verify")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> Verify([FromBody] VerifyTokenRequest req)
        {
            var verifyMapper = _mapper.Map<VerifyTokenDTO>(req);
            var result = await _playerService.Verify(verifyMapper);

            return Ok(result);
        }
    }
}
