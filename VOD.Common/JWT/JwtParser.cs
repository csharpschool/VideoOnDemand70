namespace VOD.Common.JWT;

public static class JwtParser
{
    private static byte[] ParseBase64Payload(string payload)
    {
        switch (payload.Length % 4)
        {
            case 2:
                payload += "==";
                break;
            case 3:
                payload += "=";
                break;
        }

        return Convert.FromBase64String(payload);
    }

    private static void ExtractClaimsFormPayload(List<Claim> claims, Dictionary<string, object> jwtProperties)
    {
        jwtProperties.TryGetValue(ClaimTypes.Role, out var roles);
        if (roles is null) return;

        var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');

        if (parsedRoles.Length == 0) claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));

        foreach (var parsedRole in parsedRoles)
            claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));

        jwtProperties.Remove(ClaimTypes.Role);

        claims.AddRange(jwtProperties.Select(jp => new Claim(jp.Key, jp.Value.ToString() ?? string.Empty)));
    }

    public static List<Claim> ParseClaimsFromPayload(string jwt)
    {
        var claims = new List<Claim>();
        if (string.IsNullOrWhiteSpace(jwt)) return claims;
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64Payload(payload);
        var jwtProperties = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        ExtractClaimsFormPayload(claims, jwtProperties);
        return claims;
    }

    public static SignUpUserDTO? ParseUserInfoFromPayload(string jwt)
    {
        try
        {
            var claims = ParseClaimsFromPayload(jwt);
            var email = claims.SingleOrDefault(c => c.Type.Equals("email"))?.Value.ToString() ?? string.Empty;

            return new SignUpUserDTO(email, claims);
        }
        catch (Exception ex)
        {
        }

        return null;
    }

    public static bool ParseIsInRoleFromPayload(string jwt, string role)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jwt)) return false;

            List<Claim>? claims = ParseUserInfoFromPayload(jwt)?.Roles;

            if (claims is null || claims.Count.Equals(0)) return false;

            var isInRole = claims.Exists(c => c.Value.Equals(role));

            return isInRole;
        }
        catch (Exception ex)
        {
        }

        return false;
    }

    public static bool ParseIsNotInRoleFromPayload(string jwt, string role) => !ParseIsInRoleFromPayload(jwt, role);

    public static bool CompareTokenClaims(string? token1, string? token2)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token1) || string.IsNullOrWhiteSpace(token2)) return false;

            var claims1 = ParseClaimsFromPayload(token1).Where(c => c.Type.Equals(ClaimTypes.Role));
            var claims2 = ParseClaimsFromPayload(token2).Where(c => c.Type.Equals(ClaimTypes.Role));

            if(claims1.Count() != claims2.Count()) return false;

            var success = true;
            foreach (var claim in claims1)
            {
                if (!claims2.Any(c => c.Value.Equals(claim.Value))) { success = false; break; }
            }
                
            return success;

        }
        catch { return false; }
    }
}
