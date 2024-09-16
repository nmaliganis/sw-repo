using sw.common.dtos.Vms.Accounts;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Controlles.Base;
using sw.infrastructure.Extensions;
using sw.infrastructure.Helpers.Security;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.PropertyMappings.TypeHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.common.dtos.Vms.Users;
using sw.infrastructure.Security;

namespace sw.auth.api.Controllers.API.V1;

/// <summary>
/// Class : UserJwtController
/// </summary>
[ApiVersionNeutral]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class UserJwtController : BaseController {
    private readonly IMediator _mediator;
    private readonly IUrlHelper _urlHelper;
    private readonly ITypeHelperService _typeHelperService;
    private readonly IPropertyMappingService _propertyMappingService;

    private readonly IConfiguration _configuration;

    /// <summary>
    /// Ctor : UserJwtController
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="urlHelper"></param>
    /// <param name="typeHelperService"></param>
    /// <param name="propertyMappingService"></param>
    /// <param name="configuration"></param>
    public UserJwtController(
        IMediator mediator,
        IUrlHelper urlHelper,
        ITypeHelperService typeHelperService,
        IPropertyMappingService propertyMappingService,
        IConfiguration configuration) {
        this._mediator = mediator;
        this._urlHelper = urlHelper;
        this._typeHelperService = typeHelperService;
        this._propertyMappingService = propertyMappingService;
        this._configuration = configuration;
    }

    /// <summary>
    /// Method : SetUserJwtAsync
    /// </summary>
    /// <param name="loginVm"></param>
    /// <returns></returns>
    [Route("authtoken", Name = "SetUserJwtRoute")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SetUserJwtAsync([FromBody] LoginUiModel loginVm) 
    {
        var fetchedUser = await this._mediator.Send(
            new GetUserAuthJwtTokenByLoginAndPasswordQuery(loginVm.Login,
                HashHelper.Sha512(loginVm.Password + loginVm.Login)));

        if (fetchedUser.IsNull()) {
            return this.NotFound("ERROR_FETCHING_USER");
        }

        if (!fetchedUser.IsSuccess()) {
            return this.OkOrBadRequest(fetchedUser.BrokenRules);
        }

        if (fetchedUser.Status == BusinessResultStatus.Fail) {
            return this.OkOrNoResult(fetchedUser.BrokenRules);
        }

        if (fetchedUser.Model.Id == 0) {
            return this.BadRequest("WRONG_USER_PASS");
        }

        if (!fetchedUser.Model.IsActivated) {
            return this.BadRequest("USER_NOT_ACTIVATED");
        }

        Guid newRefreshedToken = Guid.NewGuid();

        var userUpdatedWithRefreshToken =
            await this._mediator.Send(
                new UpdateUserWithNewRefreshTokenCommand(fetchedUser.Model, newRefreshedToken));

        if (userUpdatedWithRefreshToken.IsNull()) {
            return this.NotFound("ERROR_GENERATING_TOKEN");
        }

        if (!userUpdatedWithRefreshToken.IsSuccess()) {
            return this.OkOrBadRequest(userUpdatedWithRefreshToken.BrokenRules);
        }

        var tokenValue = this.GenerateJwtToken(userUpdatedWithRefreshToken.Model);

        return this.Ok(new AuthUiModel {
            Token = tokenValue,
            RefreshToken = newRefreshedToken.ToString(),
        });
    }

    #region Methods : (private)

    private string GenerateJwtToken(UserUiModel registeredUser) {

        foreach (var memberDepartmentUiModel in registeredUser.Member.Departments)
        {
            registeredUser.DepartmentIds.Add(memberDepartmentUiModel.Department.Id);
            registeredUser.CompanyId = memberDepartmentUiModel.Department.Company.Id;
            foreach (var departmentRoleUiModel in memberDepartmentUiModel.Department.Roles)
            {
                registeredUser.Roles.Add(departmentRoleUiModel.Role);
            }
        }

        string userData = new AuthUserDataUiModel() {
            CompanyId = registeredUser.CompanyId,
            MemberId = registeredUser.MemberId,
            DepartmentIds = registeredUser.DepartmentIds,
        }.ObjectToJson();

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, registeredUser.MemberEmail),
            new Claim(ClaimTypes.UserData, userData),
        };

        if (!registeredUser.Roles.IsNull() && registeredUser.Login.ToUpper() == "SU") {
            registeredUser.Roles.Add(new RoleUiModel() {
                Id = 1,
                Active = true,
                CreatedDate = DateTime.Now,
                Name = "SU"
            });
        }

        claims.AddRange(registeredUser.Roles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Name)));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this._configuration.GetSection("TokenAuthentication:SecretKey").Value);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(int.Parse(this._configuration
                .GetSection("TokenAuthentication:ExpirationTimeInMinutes").Value)),
            Issuer = this._configuration.GetSection("TokenAuthentication:Issuer").Value,
            Audience = this._configuration.GetSection("TokenAuthentication:Audience").Value,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenValue = tokenHandler.WriteToken(token);
        return tokenValue;
    }

    #endregion

}// Class : UserJwtController