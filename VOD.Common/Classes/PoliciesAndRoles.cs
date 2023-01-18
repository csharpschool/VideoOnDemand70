namespace VOD.Common.Classes;

public static class UserRole
{
    public static string Admin => "Admin";
    public static string Customer => "Customer";
    public static string Registered => "Registered";
}

public static class UserPolicy
{
    public static string Admin => "Admin";
    public static string Customer => "Customer";
    public static string Registered => "Registered";
    public static string NotCustomer => "NotRegistered";
}