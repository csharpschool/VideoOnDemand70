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
            var email = claims.SingleOrDefault(c => c.ValueType.Equals("email"))?.Value.ToString() ?? string.Empty;

            return new SignUpUserDTO(email, claims);
        }
        catch (Exception ex)
        {
        }

        return null;
    }

}
