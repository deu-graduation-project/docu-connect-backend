﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Application.Features.Queries.User.GetAgencies
{
    public class GetAgenciesQueryResponse
    {
        public int TotalCount { get; set; }
        public object Agencies { get; set; }
    }
}
