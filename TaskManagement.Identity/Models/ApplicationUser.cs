using Microsoft.AspNetCore.Identity;
using TaskManagement.Identity.Models.Enums;

namespace TaskManagement.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Inactive;

    public void Activate()
    {
        Status = Status.Active;
    }
}