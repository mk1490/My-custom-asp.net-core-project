using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using My_web_app.helper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using My_web_app.api.v1.Users.Admin.Users.Model;
using My_web_app.api.v1.Users.Users.Repository;


namespace My_web_app.api.v1.Users
{
    [Route("api/admin")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private UsersRepository _repository;

        public UsersController(IConfiguration iConfiguration, MyApplicationContext appContext) : base(iConfiguration)
        {
            _repository = new UsersRepository(appContext, iConfiguration);
        }


        [HttpPost]
        [Route("login")]
        public void Login([FromBody] LoginModel loginModel)
        {
            SendResponse(_repository.Login(loginModel));
        }


        [HttpPost]
        [Route("RegisterUser")]
        public void RegisterUser([FromBody] RegisterModel registerModel)
        {
            SendResponse(_repository.RegisterUser(registerModel));
        }


        [HttpPut]
        [Authorize]
        [Route("updateUserDetails/{id}")]
        public void UpdateUserDetails(string id, [FromBody] UpdateModel updateModel)
        {
            SendResponse(_repository.UpdateUserDetails(id, updateModel));
        }


        [HttpGet]
        [Authorize]
        [Route("getAllUsers")]
        public void GetAllUsers()
        {
            SendResponse(_repository.GetAllUsers());
        }


        [HttpDelete]
        [Authorize]
        [Route("deleteUser/{id}")]
        public void DeleteUser(string id)
        {
            SendResponse(_repository.DeleteUser(id));
        }
    }
}