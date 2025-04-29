using FotokopiRandevuAPI.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetAgencies
{
    public class GetAgenciesQueryHandler : IRequestHandler<GetAgenciesQueryRequest, GetAgenciesQueryResponse>
    {
        readonly IUserService _userService;

        public GetAgenciesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetAgenciesQueryResponse> Handle(GetAgenciesQueryRequest request, CancellationToken cancellationToken)
        {
            var response=await _userService.GetAgencies(request.Page, request.Size, request.AgencyName, request.Province, request.District,request.OrderBy,request.PaperType,request.ColorOption,request.PrintType);
            return new()
            {
                TotalCount = response.TotalCount,
                Agencies = response.Agencies,
            };
        }
    }
}
