using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace My_web_app.api.v1.Users.Admin.Users.Model
{
    [Table("tbl_test_user_details")]
    public class BaseUserDetailsModel
    {
        [Key]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string ParentUserId { get; set; }

        public string Name { get; set; }
        public string Family { get; set; }
        [Required] public string NationalityCode { get; set; }
    }
}