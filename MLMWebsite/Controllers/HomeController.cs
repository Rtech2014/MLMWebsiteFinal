using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLMWebsite.Data;
using MLMWebsite.Models;
using MLMWebsite.ViewModel;
using Newtonsoft.Json;

namespace MLMWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void DoEntry()
        {
            //if Entry for the user does not exist then create it

            var entry = new LoginEntry();
            entry.ApplicationMemberId = User.getUserId();

            entry.TimeOfLastLogin = DateTime.UtcNow; //update the last login time
            _context.LoginEntries.Add(entry);
            _context.SaveChanges();
        }

        public async Task<IActionResult> Index()
        {
            //   DoEntry();

            if (User.Identity.IsAuthenticated)
            {
                //DoEntry();
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult Wait()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult WhatWeDo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("superadmin@gmail.com");
                mail.From = new MailAddress("whoever@me.com");
                mail.Subject = $"{contactViewModel.Subject}";
                mail.Body = $"{contactViewModel.Description}";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("usemydummy94@gmail.com", "dummy@123")
                };
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"The problem is : {e}");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> UnApprovedMembers()
        {
            var usersApproved = await _userManager.Users
                                        .Where(s => s.ApprovalCount < 10).ToListAsync();
            return View(usersApproved);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Report(string joinDate, string fromDate)
        {
            if (joinDate != null && fromDate != null)
            {
                //this will default to current date if for whatever reason the date supplied by user did not parse successfully

                DateTime start = DateManager.GetDate(joinDate) ?? DateTime.Now;

                DateTime end = DateManager.GetDate(fromDate) ?? DateTime.Now;

                var rangeData = await _userManager.Users.Where(x => x.JoinDate.Month == start.Month && x.JoinDate.Month == end.Month).ToListAsync();

                return View(rangeData);
            }
            else
            {
                return View(_userManager.Users.ToList());
            }

        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> HowWorks()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ReferralDetails()
        {
            var userdata = await _userManager.GetUserAsync(User);
            var userrecordcount = userdata.RecordCount;

            var referalusers = _userManager.Users
                                    .OrderByDescending(s => s.RecordCount)
                                    .Where(r => r.RecordCount < userrecordcount)
                                    .Take(10);

            return View(await referalusers.ToListAsync());
        }

        [Authorize(Roles = "InitAdmin,SuperAdmin,Admin")]
        public IActionResult Approve()
        {
            return View();
        }



        [Authorize]
        public IActionResult Treeview()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            //Loop and add the Parent Nodes.
            foreach (ApplicationUser type in _userManager.Users)
            {

                nodes.Add(new TreeViewNode { id = type.Id.ToString(), parent = "#", text = type.Name });
            }

            //Loop and add the Child Nodes.
            foreach (UserChild subType in _context.UserChildren)
            {
                nodes.Add(new TreeViewNode { id = subType.ParentUserId.ToString() + "-" + subType.Id.ToString(), parent = subType.ParentUserId.ToString(), text = subType.ApplicationUser.Name });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }

        public async Task<IActionResult> Approver(string Id)
        {
            var userToApprove = await _context.Proof.Include(s => s.ApplicationUser)
                                                    .Where(s => s.ApplicationMemberId == Id).ToListAsync();

            return View(userToApprove);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Approve()
        //{

        //    return RedirectToAction("ApproveUser","Home");
        //}

        //downline
        [Authorize]
        public async Task<IActionResult> ApprovingUser()
        {
            var downlineUsers = new List<ApplicationUser>();
            var current_user_id = User.getUserId();
            var current_user_data = await _userManager.FindByIdAsync(current_user_id);

            var allusers = _userManager.Users.Where(s =>s.RecordCount>=current_user_data.RecordCount);

            // Get all the proofs with its userid also without the current user
            var prooferusers = _context.Proof.Include(s => s.ApplicationUser)
                                            .Where(s => s.ApplicationMemberId != current_user_id);

            // Iterate all the users 
            foreach (var item in allusers)
             {
                foreach (var itemproofs in prooferusers)
                {
                    if (itemproofs.ApplicationMemberId == item.Id)
                    {

                        if (item.ApprovalCount == 10)
                        {

                        }
                        else
                        {
                            downlineUsers.Add(new ApplicationUser()
                            {
                                Id = item.Id,
                                Email = item.Email,
                                UserName = item.UserName
                            });
                        }
                    }
                }
            }


            return View(downlineUsers);
        }
        [Authorize]
        public async Task<IActionResult> ApproveUser()
        {
            var downlineUsers = new List<ApplicationUser>();
            var current_user_id = User.getUserId();
            var current_user_data = await _userManager.FindByIdAsync(current_user_id);

                               
            string leftBranch;
            string rightBranch;
            leftBranch = current_user_data.LeftBranchID;
            rightBranch = current_user_data.RightBranchID;
            for (int i = 0; i <= _userManager.Users.Count(); i++)
            {

                if (i == 0)
                {
                    var totalUsers = _userManager.Users;
                    foreach (var presentuser in totalUsers)
                    {
                        if (presentuser.Id == leftBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                        {
                            var current_referal_user = await _userManager.FindByIdAsync(leftBranch);
                            downlineUsers.Add(current_referal_user);

                            leftBranch = current_referal_user.LeftBranchID;
                            rightBranch = current_referal_user.RightBranchID;

                        }

                        else if (presentuser.Id == rightBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                        {
                            var current_referal_user = await _userManager.FindByIdAsync(rightBranch);
                            downlineUsers.Add(current_referal_user);

                            leftBranch = current_referal_user.LeftBranchID;
                            rightBranch = current_referal_user.RightBranchID;
                        }
                    }
                    return View(downlineUsers);
                }
                else
                {

                    var totalUsers = _userManager.Users;
                    foreach (var presentuser in totalUsers)
                    {
                        if (presentuser.Id == leftBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                        {
                            var current_referal_user = await _userManager.FindByIdAsync(leftBranch);
                            downlineUsers.Add(current_referal_user);

                            leftBranch = current_referal_user.LeftBranchID;
                            rightBranch = current_referal_user.RightBranchID;

                        }
                        else if (presentuser.Id == rightBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                        {
                            var current_referal_user = await _userManager.FindByIdAsync(rightBranch);
                            downlineUsers.Add(current_referal_user);


                            leftBranch = current_referal_user.LeftBranchID;
                            rightBranch = current_referal_user.RightBranchID;
                        }
                    }
                    //return View(downlineUsers);
                }
                for (int j = 0; j <= _userManager.Users.Count(); j++)
                {
                    if (j == 0)
                    {
                        var totalUsers = _userManager.Users;
                        foreach (var presentuser in totalUsers)
                        {
                            if (presentuser.Id == rightBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                            {
                                var current_referal_user = await _userManager.FindByIdAsync(rightBranch);
                                
                                downlineUsers.Add(current_referal_user);


                                leftBranch = current_referal_user.LeftBranchID;
                                rightBranch = current_referal_user.RightBranchID;
                            }

                            //else if (presentuser.Id == leftBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                            //{
                            //    var current_referal_user = await _userManager.FindByIdAsync(leftBranch);
                            //    downlineUsers.Add(current_referal_user);

                            //    leftBranch = current_referal_user.LeftBranchID;

                            //}

                        }
                        //return View(downlineUsers);
                    }
                    else
                    {

                        var totalUsers = _userManager.Users;
                        foreach (var presentuser in totalUsers)
                        {
                            if (presentuser.Id == rightBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                            {
                                var current_referal_user = await _userManager.FindByIdAsync(rightBranch);
                                downlineUsers.Add(current_referal_user);


                                leftBranch = current_referal_user.LeftBranchID;
                                rightBranch = current_referal_user.RightBranchID;
                            }

                            else if (presentuser.Id == leftBranch && presentuser.JoinDate > current_user_data.JoinDate && presentuser.ApprovalCount < 10)
                            {
                                var current_referal_user = await _userManager.FindByIdAsync(leftBranch);
                                downlineUsers.Add(current_referal_user);

                                leftBranch = current_referal_user.LeftBranchID;
                                rightBranch = current_referal_user.RightBranchID;
                            }
                        }
                        //return View(downlineUsers);
                    }
                }
            }
            return View(downlineUsers);
            
        }

        

        [Authorize]
        public async Task<IActionResult> ReferalUsers()
        {
            var userdata = await _userManager.GetUserAsync(User);
            var userrecordcount = userdata.RecordCount;

            var referalusers = _userManager.Users
                                    .OrderByDescending(s => s.RecordCount)
                                    .Where(r => r.RecordCount > userrecordcount);
            return View(await referalusers.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    public class DateManager
    {
        /// <summary>
        /// Use to prevent month from being overritten when day is less than or equal 12
        /// </summary>
        static bool IsMonthAssigned { get; set; }



        public static DateTime? GetDate(string d)
        {
            char[] splitsoptions = { '/', '-', ' ' };
            foreach (var i in splitsoptions)
            {
                var y = 0;
                var m = 0;
                var day = 0;
                if (d.IndexOf(i) > 0)
                {
                    try
                    {
                        foreach (var e in d.Split(i))
                        {


                            if (e.Length == 4)
                            {
                                y = Convert.ToInt32(e);

                                continue;
                            }
                            if (Convert.ToInt32(e) <= 12 && !IsMonthAssigned)
                            {
                                m = Convert.ToInt32(e);
                                IsMonthAssigned = true;
                                continue;
                            }
                            day = Convert.ToInt32(e);


                        }

                        return new DateTime(y, m, day);
                    }
                    catch
                    {
                        //We are silent about this but we  could set a message about wrong date input in ViewBag    and display to user if this  this method returns null
                    }
                }
            }
            return null;


        }
        // Another overload. this will catch more date formats without manually checking as above

        public static DateTime? GetDate(string d, bool custom)
        {
            CultureInfo culture = new CultureInfo("en-US");

            string[] dateFormats =
            {
                "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "yyyy/dd/MM", "dd-MM-yyyy", "MM-dd-yyyy", "yyyy-MM-dd",
                "yyyy-dd-MM", "dd MM yyyy", "MM dd yyyy", "yyyy MM dd", "yyyy dd MM", "dd.MM.yyyy", "MM.dd.yyyy",
                "yyyy.MM.dd", "yyyy.dd.MM","yyyyMMdd","yyyyddMM","MMddyyyy","ddMMyyyy"
            };//add your own to the array if any

            culture.DateTimeFormat.SetAllDateTimePatterns(dateFormats, 'Y');

            if (DateTime.TryParseExact(d, dateFormats, culture, DateTimeStyles.None, out var date))
                return date;

            return null;


        }
    }
}