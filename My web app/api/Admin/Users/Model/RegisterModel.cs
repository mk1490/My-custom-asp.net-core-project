namespace My_web_app.api.v1.Users.Admin.Users.Model
{
    public class RegisterModel
    {
        public BaseUsersModel UserInfo { get; set; }
        public BaseUserDetailsModel UserDetails { get; set; }
        public BasePasswordModel Password { get; set; }
    }
}