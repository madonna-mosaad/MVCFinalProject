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
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Users> _userManager;

        public RoleController(IMapper mapper,RoleManager<IdentityRole> roleManager,UserManager<Users> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                var mapped = await _roleManager.Roles.Select(R=> new RoleViewModel()
                {
                    Name=R.Name,
                    Id=R.Id
                }).ToListAsync();
                return View(mapped);
            }
            else
            {
                var Result = await _roleManager.FindByNameAsync(Name);
                if(Result != null)
                {
                    var mapped = _mapper.Map<IdentityRole,RoleViewModel>(Result);
                    return View(new List<RoleViewModel> { mapped});
                }
            }
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid) 
            {
                var mapped= _mapper.Map<RoleViewModel,IdentityRole>(roleViewModel);
                var Result= await _roleManager.CreateAsync(mapped);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleViewModel);
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is not null)
            {
                var Result = await _roleManager.FindByIdAsync(id);
                if (Result is not null)
                {

                    var mapped = new RoleViewModel()
                    {
                       Name=Result.Name,
                       Id=id
                    };
                    return View(ViewName, mapped);
                }
                return NotFound();
            }
            return BadRequest();
        }
        public Task<IActionResult> Edit(string id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //make this instead of mapping because I can change only some columns
                var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
                role.Name = roleViewModel.Name;
                var Result = await _roleManager.UpdateAsync(role);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(roleViewModel);
        }
        public Task<IActionResult> Delete(string id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel roleViewModel)
        {
            if (roleViewModel is not null)
            {
                var role = await _roleManager.FindByNameAsync(roleViewModel.Name);
                if (role is not null)
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);
                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            var r = await _userManager.RemoveFromRoleAsync(user, role.Name);
                            
                        }
                    }
                    
                    var res = await _roleManager.DeleteAsync(role);
                    if (res.Succeeded)
                    {
                       return RedirectToAction("Index");
                    }
                    

                }
            }
            return View(roleViewModel);
        }
    }
}
