using AutoMapper;

using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<Users> _userManager;
		private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<Users> userManager, SignInManager<Users> signInManager,RoleManager<IdentityRole> roleManager,IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
		public async Task<IActionResult> Index(string Email)
		{
			if (string.IsNullOrWhiteSpace(Email))
			{
				//to Map from Model to ViewModel to each User
				var users = await _userManager.Users.Select(U => new UsersViewModel 
				{
					Email = U.Email,
					FName = U.FName,
					LName = U.LName,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result,
					Id = U.Id
				}).ToListAsync();//convert from IQuaryable to any collection 
				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(Email);
				if (user is not null)
				{
					var mapped = new UsersViewModel
					{
						Email = user.Email,
						FName = user.FName,
						LName = user.LName,
						PhoneNumber = user.PhoneNumber,
						Roles = _userManager.GetRolesAsync(user).Result,
						Id = user.Id
					};
					//convert to list because view is bind to IEnumrable<UsersViewMode> (@model IEnumrable<UsersViewMode>)
					return View(new List<UsersViewModel> { mapped });
				}
			}
			return View();
		}
		public IActionResult Create()
		{
			ViewData["Roles"] = _roleManager.Roles;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(UsersViewModel usersViewModel)
		{
			if (ModelState.IsValid)
			{
				var mapped = new Users
				{
					Email = usersViewModel.Email,
					FName = usersViewModel.FName,
					LName = usersViewModel.LName,
					PhoneNumber = usersViewModel.PhoneNumber,
					UserName = usersViewModel.FName,
					
					Id = usersViewModel.Id
				};
				foreach (var rolename in usersViewModel.Roles) {
					var roleExists = await _roleManager.RoleExistsAsync(rolename);
					if (!roleExists)
					{
						return BadRequest("Role does not exist.");
					} 
				}
                var Result = await _userManager.CreateAsync(mapped,usersViewModel.Password);
				if (Result.Succeeded)
				{
					var result = await _userManager.AddToRolesAsync(mapped, usersViewModel.Roles);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
				}
			}
			return View(usersViewModel);
		}
		public async Task<IActionResult> Details(string id,string ViewName="Details")
		{
		     if (id is not null)
			 {
				var Result =await _userManager.FindByIdAsync(id);
				if(Result is  not null)
				{

                    var mapped = new UsersViewModel
                    {
                        Email = Result.Email,
                        FName = Result.FName,
                        LName = Result.LName,
                        PhoneNumber = Result.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(Result).Result,
                        Id = Result.Id
                    };
                    return View(ViewName,mapped);
				}
				return NotFound();
			 }
			 return BadRequest();
		}
		public Task<IActionResult> Edit(string id)
		{
			return Details(id,"Edit");
		}
		[HttpPost]
		public async Task<IActionResult> Edit(UsersViewModel usersViewModel)
		{
			if (ModelState.IsValid)
			{
				//make this instead of mapping because I can change only some columns (as I want to send to DB same old change password) 
				var user = await _userManager.FindByIdAsync(usersViewModel.Id);
				user.PhoneNumber = usersViewModel.PhoneNumber;
				user.Email = usersViewModel.Email;
				user.LName = usersViewModel.LName;
				user.FName = usersViewModel.FName;
                foreach (var rolename in usersViewModel.Roles)
                {
					if (rolename != null)
					{
						var roleExists = await _roleManager.RoleExistsAsync(rolename);
						if (!roleExists)
						{
							return BadRequest("Role does not exist.");
						}
					}
                }
                var Result = await _userManager.UpdateAsync(user);
				if (Result.Succeeded)
				{
					var roles = await _userManager.GetRolesAsync(user);
					if (roles.Count!=0)
					{
						var res = await _userManager.RemoveFromRolesAsync(user, roles);
						
                    }
                    var result = await _userManager.AddToRolesAsync(user, usersViewModel.Roles);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index");
                    }
                    
                    

                }
			}
			return View(usersViewModel);
		}
		public Task<IActionResult> Delete(string id)
		{
			return Details(id, "Delete");
		}
		[HttpPost]
        public async Task<IActionResult> Delete(UsersViewModel usersViewModel)
        {
            if(usersViewModel is not null)
			{
                var user = await _userManager.FindByEmailAsync(usersViewModel.Email);
				if (user != null)
				{
					if (usersViewModel.Roles.Count!=0)
					{
						var result = await _userManager.RemoveFromRolesAsync(user, usersViewModel.Roles);
                        
                    }

                    var Result = await _userManager.DeleteAsync(user);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
			return View(usersViewModel);
        }
    }
}






