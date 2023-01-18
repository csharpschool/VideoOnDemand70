using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.Classes;
using VOD.Common.DTOs;

namespace VOD.Users.API.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<VODUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UsersController(UserManager<VODUser> userManager, RoleManager<IdentityRole> roleManager) => (_userManager, _roleManager) = (userManager, roleManager);

    private async Task<IResult> AddUser(string email, string password)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null) return Results.BadRequest();

            VODUser newUser = new()
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded) return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    private async Task<IResult> AddRoles(string email, List<string> roles)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Results.BadRequest();

            IdentityResult result = await _userManager.AddToRolesAsync(user, roles);

            if (result.Succeeded) return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/users/seed")]
    [HttpPost]
    public async Task<IResult> Seed()
    {
        try
        {
            await _roleManager.CreateAsync(new IdentityRole { Id = "1", Name = UserRole.Admin });
            await _roleManager.CreateAsync(new IdentityRole { Id = "2", Name = UserRole.Customer });
            await _roleManager.CreateAsync(new IdentityRole { Id = "3", Name = UserRole.Registered });

            var john = "john@vod.com";
            var jane = "jane@vod.com";
            var password = "Pass123__";

            await AddUser(john, password);
            await AddRoles(john, new List<string> { UserRole.Admin, UserRole.Customer, UserRole.Registered });
            await AddUser(jane, password);
            await AddRoles(jane, new List<string> { UserRole.Customer, UserRole.Registered });

            return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/users/register")]
    [HttpPost]
    public async Task<IResult> Register(RegisterUserDTO registerUserDTO)
    {
        try
        {
            var result = await AddUser(registerUserDTO.Email, registerUserDTO.Password);

            if (result.Equals(Results.BadRequest())) return Results.BadRequest();

            result = await AddRoles(registerUserDTO.Email, registerUserDTO.Roles);
            
            return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/users/paid")]
    [HttpPost]
    public async Task<IResult> Paid(PaidCustomerDTO paidCustomerDTO) => 
        await AddRoles(paidCustomerDTO.Email, new List<string> { UserRole.Customer });
    
}