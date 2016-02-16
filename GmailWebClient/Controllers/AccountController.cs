using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GmailWebClient.Entities;
using GmailWebClient.Mappers;
using GmailWebClient.Models;
using GmailWebClient.Models.Repositories;
using GmailWebClient.Utilities;
using Microsoft.Practices.Unity;

namespace GmailWebClient.Controllers
{
    public class AccountController : Controller
    {
        [Dependency]
        public IUserMappper UserMapper { get; set; }

        [Dependency]
        public IUserRepository UserRepository { get; set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserRepository.GetByUserName(model.UserName);
                if (user != null)
                {
                    if (ValidateUser(user, model))
                    {
                        //Proceed
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!UserExits(model))
                {
                    // Attempt to register the user
                    try
                    {
                        model.Password = PasswordHash.CreateHash(model.Password);
                        var user = UserMapper.MapToUser(model, new User());
                        try
                        {
                            var userEntity = UserRepository.Create(user);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", "Couldn't Register the user");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username is already registered");
                }
               
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RegisterGmailData()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterGmailData(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Update user with Gmail Data
                try
                {
                    var user = UserRepository.GetByUserName(User.Identity.Name);
                    user.GmailUser = model.UserName;
                    user.GmailPassword = model.Password;
                    try
                    {
                        UserRepository.Update(user);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", "Couldn't Save the user");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private bool UserExits(RegisterModel model)
        {
            return UserRepository.GetByUserName(model.UserName) != null;
        }

        private bool ValidateUser(User user, LoginModel model)
        {
            return PasswordHash.ValidatePassword(model.Password, user.Password);
        }
    }
}
