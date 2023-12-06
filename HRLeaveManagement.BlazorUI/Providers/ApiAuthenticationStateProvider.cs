using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRLeaveManagement.BlazorUI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _jwtSecurityToken;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            this._localStorage = localStorage;
            this._jwtSecurityToken = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var isTokenPresent = await _localStorage.ContainKeyAsync("token");
            if (!isTokenPresent)
            {
                return new AuthenticationState(user);
            }

            var savedToken = await _localStorage.GetItemAsync<string>("token");
            var tokenContent = _jwtSecurityToken.ReadJwtToken(savedToken);

            if (tokenContent.ValidTo < DateTime.Now)
            {
                await _localStorage.RemoveItemAsync("token");
                return new AuthenticationState(user);
            }

            var claims = await GetClaimsAsync();
            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        public async Task LoggedIn()
        {
            var claims = await GetClaimsAsync();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoggedOut()
        {
            await _localStorage.RemoveItemAsync("token");
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<List<Claim>> GetClaimsAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("token");
            var tokenContent = _jwtSecurityToken.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims.ToList();

            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
