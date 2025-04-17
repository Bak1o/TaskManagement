using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Identity.Requests;

public sealed class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
}