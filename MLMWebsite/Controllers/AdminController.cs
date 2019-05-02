using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;
using MLMWebsite.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MLMWebsite.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        //private readonly UserChild _userChild;
       
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context)
        {
            
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
           //this. _userChild = userChild;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            return View();
        }
        public IActionResult UserManagement()
        {
            var users = _userManager.Users;
            return View(users);
        }

        public IActionResult AddUser()
        {
            if (User.IsInRole("InitAdmin"))
            {
                ViewData["InitAdmin"] = "You are initializer admin";
            }
            return View();
        }

        public async Task<string> UpdateCurrentUserLeft(string leftBranchId)
        {
            var updateuser = await _userManager.GetUserAsync(User);
            var user = await _userManager.FindByIdAsync(updateuser.Id);

            try
            {
                
                //user.Pathfile = updateuser.Pathfile + user.UserName;
                updateuser.LeftBranchID = leftBranchId;
                updateuser.branchcount++;

                //_userChild.ChildId = leftBranchId;
                //_userChild.ParentUserId = updateuser.Id;

               
                var result = await _userManager.UpdateAsync(updateuser);
            }
            catch (Exception)
            {

                throw;
            }

            return "super";
        }

        public async Task<string> UpdateCurrentUserRight(string rightBranchId)
        {
            var updateuser = await _userManager.GetUserAsync(User);
            var user = await _userManager.FindByIdAsync(updateuser.Id);

            try
            {
                //user.Pathfile = updateuser.Pathfile + user.UserName;
                updateuser.RightBranchID = rightBranchId;
                updateuser.branchcount++;

                //_userChild.ChildId = rightBranchId;
                //_userChild.ParentUserId = updateuser.Id;

                var result = await _userManager.UpdateAsync(updateuser);
            }
            catch (Exception)
            {

                throw;
            }

            return "super";
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            var Userdata = await _userManager.GetUserAsync(User);


            #region Super Admin Add User
            if (User.IsInRole("SuperAdmin") && Userdata.branchcount == 0)
            {
                var user = new ApplicationUser()
                {
                    UserName = addUserViewModel.UserName,
                    Email = addUserViewModel.Email,
                    Pathfile = addUserViewModel.UserName + Userdata.Pathfile,
                    ParentID = Userdata.Id,

                };

                IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

                if (result.Succeeded)
                {
                    Userdata.branchcount += 1;
                    var userrole = await _userManager.AddToRoleAsync(user, "Admin");

                    try
                    {
                        MailMessage mail = new MailMessage();
                        //Debug.WriteLine($"************************{currentemail}*******************");
                        mail.To.Add(addUserViewModel.Email);
                        mail.From = new MailAddress("whoever@me.com");
                        mail.Subject = "About Join Hand Account Details";
                        mail.Body = $"Your User name is :  {addUserViewModel.UserName} & your password is : {addUserViewModel.Password} <br>";
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("usemydummy94@gmail.com", "dummy@123");
                        smtp.Send(mail);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"The problem is : {e}");
                    }

                    await _userManager.UpdateAsync(Userdata);
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(addUserViewModel);
            }
            #endregion

            #region Admin Adduser
            if (User.IsInRole("Admin") && Userdata.branchcount == 0 )
            {
                var user = new ApplicationUser()
                {
                    UserName = addUserViewModel.UserName,
                    Email = addUserViewModel.Email,
                    Pathfile = addUserViewModel.UserName + Userdata.Pathfile,
                    ParentID = Userdata.Id,

                };

                IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

                if (result.Succeeded)
                {
                    Userdata.branchcount += 1;
                    var userrole = await _userManager.AddToRoleAsync(user, "Admin");

                    try
                    {
                        MailMessage mail = new MailMessage();
                        //Debug.WriteLine($"************************{currentemail}*******************");
                        mail.To.Add(addUserViewModel.Email);
                        mail.From = new MailAddress("whoever@me.com");
                        mail.Subject = "About Join Hand Account Details";
                        mail.Body = $"Your User name is :  {addUserViewModel.UserName} & your password is : {addUserViewModel.Password} <br>";
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("usemydummy94@gmail.com", "dummy@123");
                        smtp.Send(mail);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"The problem is : {e}");
                    }

                    await _userManager.UpdateAsync(Userdata);
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(addUserViewModel);
            }
            else
            {
                if (User.IsInRole("InitAdmin") && Userdata.LeftBranchID == null && Userdata.branchcount <= 2)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = addUserViewModel.UserName,
                        Email = addUserViewModel.Email,
                        ParentID = Userdata.Id,
                        Pathfile = addUserViewModel.UserName + Userdata.UserName
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

                    if (result.Succeeded)
                    {
                        UpdateCurrentUserLeft(user.Id).Wait();
                        ApprovedUser app = new ApprovedUser();
                        app.UserId = user.Id;
                        app.ApproverId = null;
                        _context.ApprovedUsers.Add(app);
                        _context.SaveChanges();
                        try
                        {
                            MailMessage mail = new MailMessage();
                            //Debug.WriteLine($"************************{currentemail}*******************");
                            mail.To.Add(addUserViewModel.Email);
                            mail.From = new MailAddress("whoever@me.com");
                            mail.Subject = "About Join Hand Account Details";
                            mail.Body = $"Your User name is :  {addUserViewModel.UserName} & your password is : {addUserViewModel.Password} <br>";
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("usemydummy94@gmail.com", "dummy@123");
                            smtp.Send(mail);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"The problem is : {e}");
                        }

                        return RedirectToAction("Index", "Home");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(addUserViewModel);
                }
                else if (Userdata.RightBranchID == null && Userdata.LeftBranchID != null && Userdata.branchcount <= 2)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = addUserViewModel.UserName,
                        Email = addUserViewModel.Email,
                        ParentID = Userdata.Id,
                        Pathfile = addUserViewModel.UserName + Userdata.UserName
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

                    if (result.Succeeded)
                    {
                        UpdateCurrentUserRight(user.Id).Wait();
                        try
                        {
                            MailMessage mail = new MailMessage();
                            //Debug.WriteLine($"************************{currentemail}*******************");
                            mail.To.Add(addUserViewModel.Email);
                            mail.From = new MailAddress("whoever@me.com");
                            mail.Subject = "About Join Hand Account Details";
                            mail.Body = $"Your User name is :  {addUserViewModel.UserName} & your password is : {addUserViewModel.Password} <br>";
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new System.Net.NetworkCredential("usemydummy94@gmail.com", "dummy@123");
                            smtp.Send(mail);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"The problem is : {e}");
                        }

                        return RedirectToAction("Index", "Home");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(addUserViewModel);
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            #endregion
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction("UserManagement", _userManager.Users);

            var claims = await _userManager.GetClaimsAsync(user);
            var vm = new EditUserViewModel() { Id = user.Id, Email = user.Email, UserName = user.UserName };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

            if (user != null)
            {
                user.Email = editUserViewModel.Email;
                user.UserName = editUserViewModel.UserName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("UserManagement", _userManager.Users);

                ModelState.AddModelError("", "User not updated, something went wrong.");

                return View(editUserViewModel);
            }
            return RedirectToAction("UserManagement", _userManager.Users);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UserManagement");
                else
                    ModelState.AddModelError("", "Something went wrong while deleting this user.");
            }
            else
            {
                ModelState.AddModelError("", "This user can't be found");
            }
            return View("UserManagement", _userManager.Users);
        }



        //Roles management
        public IActionResult RoleManagement()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult AddNewRole() => View();

        [HttpPost]
        public async Task<IActionResult> AddNewRole(AddRoleViewModel addRoleViewModel)
        {

            if (!ModelState.IsValid) return View(addRoleViewModel);

            var role = new IdentityRole
            {
                Name = addRoleViewModel.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addRoleViewModel);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };


            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    editRoleViewModel.Users.Add(user.UserName);
            }

            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
            {
                role.Name = editRoleViewModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", _roleManager.Roles);

                ModelState.AddModelError("", "Role not updated, something went wrong.");

                return View(editRoleViewModel);
            }

            return RedirectToAction("RoleManagement", _roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", _roleManager.Roles);
                ModelState.AddModelError("", "Something went wrong while deleting this role.");
            }
            else
            {
                ModelState.AddModelError("", "This role can't be found.");
            }
            return View("RoleManagement", _roleManager.Roles);
        }

        //Users in roles
        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id };

            foreach (var user in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        public async Task<IActionResult> DeleteUserFromRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        public async Task<IActionResult> TreeView(string id)
        {
            var branches = new List<ApplicationUser>();
            string left, right;
            if (id == null)
            {
                id = User.getUserId();
                var baseUser = await _userManager.FindByIdAsync(id);
                branches.Add(baseUser);
                if (baseUser.LeftBranchID != null)
                {
                    left = baseUser.LeftBranchID;
                    var leftUser = await _userManager.FindByIdAsync(left);
                    branches.Add(leftUser);
                }
                else
                {
                     
                }
                if (baseUser.RightBranchID != null)
                {
                    right = baseUser.RightBranchID;
                    var rightUser = await _userManager.FindByIdAsync(right);
                    branches.Add(rightUser);
                }
                else
                {

                }
                return View(branches);
            }
            else
            {
                var baseUser = await _userManager.FindByIdAsync(id);
                branches.Add(baseUser);
                if (baseUser.LeftBranchID != null)
                {
                    left = baseUser.LeftBranchID;
                    var leftUser = await _userManager.FindByIdAsync(left);
                    branches.Add(leftUser);
                }
                if (baseUser.RightBranchID != null)
                {
                    right = baseUser.RightBranchID;
                    var rightUser = await _userManager.FindByIdAsync(right);
                    branches.Add(rightUser);
                }
                return View(branches);
            }

        }




        [HttpPost]
        public async Task<IActionResult> DeleteUserFromRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }


        public async Task<IActionResult> MemberList(string Use)
        {
            var cUser = User.getUserId();
            var current = await _userManager.FindByIdAsync(cUser);
            if (Use != null)
            {
                var search =  _userManager.Users.Where(a=>a.UserName.Contains(Use));
                return View(search);
            }
            var UserData = _userManager.Users.Where(s => s.JoinDate > current.JoinDate);

            return View(UserData);
            
            //else
            //{
            //    return View(_userManager.Users);
            //}

        }
        public async Task<IActionResult> SendNotification(ContactViewModel contactViewModel,string email)
        {
            var id = User.getUserId();
            var curuser = _userManager.Users.Where(s => s.Id == id).FirstOrDefault();
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(curuser.Email);
            mail.Subject = $"Remembrance for Approving";
            mail.Body = $"Dear Sir, My name is {curuser.UserName} and my mail Id is {curuser.Email} I have Upload my proof. So,Please approve me as soon as possible";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("mailbyglobalcare@gmail.com", "Ram@1234")
            };
            smtp.Send(mail);

            return RedirectToAction("Notify","Admin");
        }

        public async Task<IActionResult> ViewQrcode()
        {
            var data = _context.BarCodes.Include(b => b.ApplicationUser);

            return View();
        }

        public async Task<IActionResult> Notify()
        {
            return View();
        }

        public async Task<IActionResult> DummyAccounterror()
        {
            return View();
        }
        public async Task<IActionResult> Upline()
        {

            var uplineUsers = new List<ApplicationUser>();
            var current_user_id = User.getUserId();
            var current_user_data = await _userManager.FindByIdAsync(current_user_id);

            string referalId;

            //var superAdmin = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            //uplineUsers.Add(superAdmin);

            referalId = current_user_data.ParentID;
            for (int i = 0; i < 10; i++)
            {
                 
                
                var totalUsers = _userManager.Users;
                foreach (var presentuser in totalUsers)
                {
                    if (presentuser.Id == referalId &&uplineUsers.Count()<10)
                    {
                       var current_referal_user = await _userManager.FindByIdAsync(referalId);
                        uplineUsers.Add(current_referal_user);

                        referalId = current_referal_user.ParentID;
                    }

                }
               
            }
            var superadmin = _userManager.Users.Where(s => s.ParentID == null).FirstOrDefault();
            uplineUsers.Add(superadmin);
            return View(uplineUsers);
            //var totalAdmin = _userManager.Users;
            //var admincount = 10 - uplineUsers.Count;


            //for (int j = 0; j <= admincount; j++)
            //{
            //    foreach (var presentuser in totalAdmin)
            //    {
            //        //if (User.IsInRole("Admin"))
            //        //{
            //        //    admin = await _userManager.GetUsersInRoleAsync("Admin");


            //        //}
            //    }


            //}
            ////uplineUsers.AddRange();


            //return View(uplineUsers);
        }
    }
}