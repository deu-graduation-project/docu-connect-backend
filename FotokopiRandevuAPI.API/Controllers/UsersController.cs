using FotokopiRandevuAPI.Application.Abstraction.Services;
using FotokopiRandevuAPI.Application.Features.Commands.User.AssignToRole;
using FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgency;
using FotokopiRandevuAPI.Application.Features.Commands.User.BeAnAgencyConfirm;
using FotokopiRandevuAPI.Application.Features.Commands.User.CreateUser;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetAgencies;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetBeAnAgencyRequests;
using FotokopiRandevuAPI.Application.Features.Queries.User.GetSingleAgency;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FotokopiRandevuAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]

        public async Task<IActionResult> BeAnAgency(BeAnAgencyCommandRequest beAnAgencyCommandRequest)
        {
            BeAnAgencyCommandResponse response = await _mediator.Send(beAnAgencyCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> BeAnAgencyConfirm(BeAnAgencyConfirmCommandRequest beAnAgencyCommandRequest)
        {
            BeAnAgencyConfirmCommandResponse response = await _mediator.Send(beAnAgencyCommandRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetBeAnAgencyRequests([FromQuery]GetBeAnAgencyRequestsQueryRequest beAnAgencyCommandRequest)
        {
            GetBeAnAgencyRequestsQueryResponse response = await _mediator.Send(beAnAgencyCommandRequest);
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetAgencies([FromQuery] GetAgenciesQueryRequest getAgenciesQueryRequest)
        {
            GetAgenciesQueryResponse response = await _mediator.Send(getAgenciesQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetSingleAgency([FromQuery] GetSingleAgencyQueryRequest getSingleAgencyQueryRequest)
        {
            GetSingleAgencyQueryResponse response = await _mediator.Send(getSingleAgencyQueryRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AssignRolesToUser(AssignRolesToUserCommandRequest assignRolesToUserCommandRequest)
        {
            AssignRolesToUserCommandResponse response = await _mediator.Send(assignRolesToUserCommandRequest);
            return Ok(response);
        }
    }
}
