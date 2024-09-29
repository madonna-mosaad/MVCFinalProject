using AutoMapper;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using MVC.ViewModels;

namespace MVC.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();    
        }
    }
}
