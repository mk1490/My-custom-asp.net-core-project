using System;
using Microsoft.EntityFrameworkCore;
using My_web_app.helper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using My_web_app.api.v1.Users.Admin.Users.Model;
using My_web_app.common;

namespace My_web_app.api.v1.Users.Users.Repository
{
    public class UsersRepository : CustomBaseRepository
    {
        private MyApplicationContext _context;

        public UsersRepository(DbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _context = (MyApplicationContext) dbContext;
        }

        public ResponseHandler GetAllUsers()
        {
            var users = _context.UserModel.ToList();
            var userDetails = _context.UserDetailsModel;
            for (int i = 0; i < users.Count; i++)
            {
                var userId = users[i].UserId;
                users[i].userDetails = userDetails.ToList().Find(i => i.ParentUserId == userId);
            }

            return Ok(users.ToList());
        }


        public ResponseHandler RegisterUser(RegisterModel registerModel)

        {
            if (!registerModel.Password.Password.Equals(registerModel.Password.VerifyPassword))
                return Ok(ResponseStatus.NOT_ACCEPT, "passwords_not_equal");

            if (CheckIfUsernameExists(registerModel.UserInfo.Username))
                return Ok(ResponseStatus.NOT_ACCEPT, "username_already_exists");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userId = generateUUID();
                    registerModel.UserInfo.UserId = userId;
                    registerModel.UserInfo.Email = registerModel.UserInfo.Email;
                    registerModel.UserInfo.Username = registerModel.UserInfo.Username;
                    registerModel.UserInfo.Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password.Password);
                    registerModel.UserInfo.RegisterDate = DateTime.Now;
                    registerModel.UserDetails.ParentUserId = userId;
                    registerModel.UserDetails.Name = registerModel.UserDetails.Name;
                    registerModel.UserDetails.Family = registerModel.UserDetails.Family;
                    registerModel.UserDetails.NationalityCode = registerModel.UserDetails.NationalityCode;
                    _context.UserModel.Add(registerModel.UserInfo);
                    _context.SaveChanges();
                    _context.UserDetailsModel.Add(registerModel.UserDetails);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("register_success", ResponseStatus.OK, new {token = GenerateJwtToken(userId)});
                }
                catch
                {
                    transaction.Rollback();
                    return Ok(ResponseStatus.SERVER_ERROR, "action_failed");
                }
            }
        }


        public ResponseHandler UpdateUserDetails(string id, UpdateModel updateItem)
        {
            var user_model = _context.UserModel.FirstOrDefault(item => item.UserId == id);


            var user_details_model =
                _context.UserDetailsModel.FirstOrDefault(item => item.ParentUserId == id);

            if (user_model == null && user_details_model == null)
                return Ok(ResponseStatus.NOT_FOUND, "user_not_found");


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    user_model.Email = updateItem.Email;
                    user_details_model.Name = updateItem.UserDetails.Name;
                    user_details_model.Family = updateItem.UserDetails.Family;
                    user_details_model.NationalityCode = updateItem.UserDetails.NationalityCode;
                    _context.UserModel.Update(user_model);
                    _context.SaveChanges();
                    _context.UserDetailsModel.Update(user_details_model);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok("user_update_success", ResponseStatus.OK, null);
                }
                catch
                {
                    transaction.Rollback();
                    return Ok(ResponseStatus.SERVER_ERROR, "action_failed");
                }
            }
        }

        public ResponseHandler Login(LoginModel loginModel)
        {
            var item = _context.UserModel.Where(s =>
                s.Username.Equals(loginModel.Username)).ToList();
            if (item.Count < 1)
            {
                return Ok(ResponseStatus.NOT_FOUND, "no_user_found_or_username_invalid");
            }
            else
            {
                var userItem = item[0];
                if (BCrypt.Net.BCrypt.Verify(loginModel.Password, item[0].Password))
                {
                    return Ok("login_success", ResponseStatus.OK, new {token = GenerateJwtToken(userItem.UserId)});
                }
                else
                {
                    return Ok("password_error", ResponseStatus.AUTH_FAILED, null);
                }
            }
        }

        public ResponseHandler DeleteUser(string userId)
        {
            var user_model = _context.UserModel.FirstOrDefault(item => item.UserId == userId);
            var user_details_model = _context.UserDetailsModel.FirstOrDefault(item => item.ParentUserId == userId);
            if (user_model == null && user_details_model == null)
                return Ok(ResponseStatus.NOT_FOUND, "user_not_found");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.UserModel.Remove(user_model);
                    _context.SaveChanges();
                    _context.UserDetailsModel.Remove(user_details_model);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok(ResponseStatus.OK);
                }
                catch
                {
                    return Ok(ResponseStatus.SERVER_ERROR);
                }
            }
        }

        public bool CheckIfUsernameExists(string username)
        {
            return _context.UserModel.Any(u => u.Username == username);
        }
    }
}