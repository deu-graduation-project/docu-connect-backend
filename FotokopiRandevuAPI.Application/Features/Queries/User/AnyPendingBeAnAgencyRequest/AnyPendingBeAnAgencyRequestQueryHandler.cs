using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.AnyPendingBeAnAgencyRequest
{
    public class AnyPendingBeAnAgencyRequestQueryHandler:IRequestHandler<AnyPendingBeAnAgencyRequestQueryRequest, AnyPendingBeAnAgencyRequestQueryResponse>
    {
        readonly IUserService _userService;

        public AnyPendingBeAnAgencyRequestQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<AnyPendingBeAnAgencyRequestQueryResponse> Handle(AnyPendingBeAnAgencyRequestQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.AnyPendingBeAnAgencyRequest();
            return new()
            {
                HasRequest=response
            };
        }

    }
}
