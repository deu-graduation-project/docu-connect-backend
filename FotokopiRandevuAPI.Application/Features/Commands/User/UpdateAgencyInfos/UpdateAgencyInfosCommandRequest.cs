using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Commands.User.UpdateAgencyInfos
{
    public class UpdateAgencyInfosCommandRequest:IRequest<UpdateAgencyInfosCommandResponse>
    {
        public string? Name{ get; set; }
        public string? Surname { get; set; }

        public string? AgencyName { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Extra { get; set; }
        public string? AgencyBio { get; set; }

        [JsonIgnore]
        public IFormFile? ProfilePhoto{ get; set; }

    }
}
