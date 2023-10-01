﻿using Microsoft.AspNetCore.Identity;

namespace Tickets.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }

}
