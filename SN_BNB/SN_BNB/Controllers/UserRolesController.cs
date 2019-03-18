﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SN_BNB.Data;
using SN_BNB.Models;
using SN_BNB.ViewModels;

namespace SN_BNB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await (from u in _context.Users
                               .OrderBy(u => u.UserName)
                               select new UserVM
                               {
                                   Id = u.Id,
                                   UserName = u.UserName
                               }).ToListAsync();
            foreach (var u in users)
            {
                var user = await _userManager.FindByIdAsync(u.Id);
                u.userRoles = await _userManager.GetRolesAsync(user);
            };
            return View(users);
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var _user = await _userManager.FindByIdAsync(id);//IdentityRole
            if (_user == null)
            {
                return NotFound();
            }
            UserVM user = new UserVM
            {
                Id = _user.Id,
                UserName = _user.UserName,
                userRoles = await _userManager.GetRolesAsync(_user)
            };
            PopulateAssignedRoleData(user);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, string[] selectedRoles)
        {
            var _user = await _userManager.FindByIdAsync(Id);//IdentityRole
            UserVM user = new UserVM
            {
                Id = _user.Id,
                UserName = _user.UserName,
                userRoles = await _userManager.GetRolesAsync(_user)
            };
            if (User.Identity.Name != user.UserName)
            {
                try
                {
                    await UpdateUserRoles(selectedRoles, user);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty,
                                    "Unable to save changes.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                                  "You cannot change your own role");
            }

            PopulateAssignedRoleData(user);
            return View(user);
        }

        public struct UserStruct
        {
            public string UserEmail;
            public string UserPassword;
            public string UserRole;
        }

        [HttpPost]
        public async Task<IActionResult> ExcelUpload(IFormFile file)
        {
            //create a struct to hold user data
            List<UserStruct> dataStructs = new List<UserStruct>();

            //receive excel file
            ExcelPackage excelPackage;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    excelPackage = new ExcelPackage(memoryStream);
                }
                var worksheet = excelPackage.Workbook.Worksheets[0];

                //parse the file and update struct
                var start = worksheet.Dimension.Start;
                var end = worksheet.Dimension.End;
                for (int row = start.Row; row <= end.Row; row++)
                {
                    UserStruct tempStruct = new UserStruct
                    {
                        UserEmail = worksheet.Cells[row, 1].Text,
                        UserRole = worksheet.Cells[row, 2].Text,
                        UserPassword = worksheet.Cells[row, 3].Text,
                    };
                    dataStructs.Add(tempStruct);
                }
                //make new fixtures using the struct
                foreach (UserStruct userStruct in dataStructs)
                {
                    IdentityUser tempUser = new IdentityUser();
                    tempUser.Email = userStruct.UserEmail;
                    tempUser.UserName = userStruct.UserEmail;
                    IdentityResult result = _userManager.CreateAsync(tempUser, userStruct.UserPassword).Result;
                    if (result.Succeeded && userStruct.UserRole.Length>0)
                    {
                        _userManager.AddToRoleAsync(tempUser, userStruct.UserRole).Wait();
                    }
                    //update tables
                    _context.SaveChanges();
                }

                //update user table
                _context.SaveChanges();

            }
            //let the user know that the file was not parsed properly
            catch { }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateAssignedRoleData(UserVM user)
        {//Prepare checkboxes for all Roles
            var allRoles = _context.Roles;
            var currentRoles = user.userRoles;
            var viewModel = new List<RoleVM>();
            foreach (var r in allRoles)
            {
                viewModel.Add(new RoleVM
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    Assigned = currentRoles.Contains(r.Name)
                });
            }
            ViewBag.Roles = viewModel;
        }

        private async Task UpdateUserRoles(string[] selectedRoles, UserVM userToUpdate)
        {
            var userRoles = userToUpdate.userRoles;//Current roles use is in
            var _user = await _userManager.FindByIdAsync(userToUpdate.Id);//IdentityUser

            if (selectedRoles == null)
            {
                //No roles selected so just remove any currently assigned
                foreach (var r in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(_user, r);
                }
            }
            else
            {
                //At least one role checked so loop through all the roles
                //and add or remove as required

                //We need to do this next line because foreach loops don't always work well
                //for data returned by EF when working async.  Pulling it into an IList<>
                //first means we can safely loop over the colleciton making async calls and avoid
                //the error 'New transaction is not allowed because there are other threads running in the session'
                IList<IdentityRole> allRoles = _context.Roles.ToList<IdentityRole>();

                foreach (var r in allRoles)
                {
                    if (selectedRoles.Contains(r.Name))
                    {
                        if (!userRoles.Contains(r.Name))
                        {
                            await _userManager.AddToRoleAsync(_user, r.Name);
                        }
                    }
                    else
                    {
                        if (userRoles.Contains(r.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(_user, r.Name);
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}