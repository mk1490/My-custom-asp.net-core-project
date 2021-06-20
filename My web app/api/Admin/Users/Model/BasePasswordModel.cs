using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_web_app.api.v1.Users.Admin.Users.Model
{
    public class BasePasswordModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string VerifyPassword { get; set; }
    }
}