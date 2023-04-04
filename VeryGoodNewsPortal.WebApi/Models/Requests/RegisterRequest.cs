namespace VeryGoodNewsPortal.WebApi.Models.Requests;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Role = "User";
}
