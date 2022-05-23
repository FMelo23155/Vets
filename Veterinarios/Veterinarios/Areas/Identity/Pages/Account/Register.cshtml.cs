// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using Veterinarios.Data;
using Veterinarios.Models;

namespace Veterinarios.Areas.Identity.Pages.Account {

   public class RegisterModel : PageModel {

      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserStore<ApplicationUser> _userStore;
      private readonly IUserEmailStore<ApplicationUser> _emailStore;
      private readonly ILogger<RegisterModel> _logger;
      private readonly IEmailSender _emailSender;

      /// <summary>
      /// creates a 'connection' to our database
      /// </summary>
      private readonly ApplicationDbContext _context;

      public RegisterModel(
          UserManager<ApplicationUser> userManager,
          IUserStore<ApplicationUser> userStore,
          SignInManager<ApplicationUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          ApplicationDbContext context) {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;
         _logger = logger;
         _emailSender = emailSender;
         _context = context;
      }

      /// <summary>
      /// define the interface data
      /// </summary>
      [BindProperty]
      public InputModel Input { get; set; }

      /// <summary>
      /// link to redirect user after registration
      /// </summary>
      public string ReturnUrl { get; set; }

      /// <summary>
      /// if you have External Login providers, you will see it here
      /// </summary>
      public IList<AuthenticationScheme> ExternalLogins { get; set; }

      /// <summary>
      /// define the data that is used at interface
      /// </summary>
      public class InputModel {

         [Required]
         [EmailAddress]
         [Display(Name = "Email")]
         public string Email { get; set; }

         [Required]
         [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
         [DataType(DataType.Password)]
         [Display(Name = "Password")]
         public string Password { get; set; }

         [DataType(DataType.Password)]
         [Display(Name = "Confirm password")]
         [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
         public string ConfirmPassword { get; set; }

         ///// <summary>
         ///// personal user name
         ///// </summary>
         //[Required]
         //public string Name { get; set; }

         /// <summary>
         /// collect the owner's data
         /// </summary>
         public Owner Owner { get; set; }

      }

      /// <summary>
      /// reacts to HTTP GET action
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public async Task OnGetAsync(string returnUrl = null) {
         ReturnUrl = returnUrl;
         ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
      }


      /// <summary>
      /// reacts to HTTP POST action
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
      public async Task<IActionResult> OnPostAsync(string returnUrl = null) {

         returnUrl ??= Url.Content("~/");

         // we are not going to use the external authentication
         //   ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

         if (ModelState.IsValid) {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            // add Name and RegistrationDate to the 'user
            user.Name = Input.Owner.Name;
            user.RegistrationDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded) {
               _logger.LogInformation("User created a new account with password.");

               // all users registed by this way are 'Clients'
               await _userManager.AddToRoleAsync(user, "Client");


               // **********************************************************
               // save the owner's data
               // **********************************************************
               // add data that is missing from owner's data
               Input.Owner.Email = Input.Email;
               Input.Owner.UserID = user.Id;

               try {
                  _context.Add(Input.Owner);
                  await _context.SaveChangesAsync();
               }
               catch (Exception) {
                  // if I am here, something bad happened
                  // what I need to do????
                  // 
                  // I must do a Rollback to all process
                  // this mean - delete the user prevously created
                  await _userManager.DeleteAsync(user);
                  // create a message to user
                  ModelState.AddModelError("", "It was impossible to create user. Something wrong happened");
                  // return control to user
                  return Page();
               }

               // email validation
               var userId = await _userManager.GetUserIdAsync(user);
               var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
               var callbackUrl = Url.Page(
                   "/Account/ConfirmEmail",
                   pageHandler: null,
                   values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                   protocol: Request.Scheme);

               await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

               if (_userManager.Options.SignIn.RequireConfirmedAccount) {
                  return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
               }
               else {
                  await _signInManager.SignInAsync(user, isPersistent: false);
                  return LocalRedirect(returnUrl);
               }
            }
            foreach (var error in result.Errors) {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         // If we got this far, something failed, redisplay form
         return Page();
      }

      private ApplicationUser CreateUser() {
         try {
            return Activator.CreateInstance<ApplicationUser>();
         }
         catch {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
         }
      }

      private IUserEmailStore<ApplicationUser> GetEmailStore() {
         if (!_userManager.SupportsUserEmail) {
            throw new NotSupportedException("The default UI requires a user store with email support.");
         }
         return (IUserEmailStore<ApplicationUser>)_userStore;
      }
   }
}
