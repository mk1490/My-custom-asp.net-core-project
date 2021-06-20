using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace My_web_app.api.v1.Users.Admin.Users.Model
{
    [Table("tbl_test_users")]
    public class BaseUsersModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Key]
        public string UserId { get; set; } // UUID user id;

        [Required] public string Username { get; set; } // Username;
        public string Email { get; set; } // Username;

        [Column("hashPassword")] [JsonIgnore] public string Password { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public DateTime RegisterDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public BaseUserDetailsModel userDetails { get; set; }
    }
}