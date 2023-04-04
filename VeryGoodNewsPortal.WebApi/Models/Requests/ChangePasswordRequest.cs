namespace VeryGoodNewsPortal.WebApi.Models.Requests;

public class ChangePasswordRequest
{
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}