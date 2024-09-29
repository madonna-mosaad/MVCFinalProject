using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Repository;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class DepartmentController : Controller
    {
        //after use UnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        //in GenericRepository before use UnitOfWork
        //private readonly IDepartmentRepository _repository;

        private readonly IWebHostEnvironment _environment;//registered in AddControllerWithView as (Singelton) tomake developer log any exception 
        private readonly IMapper _mapper;
        
        public DepartmentController(/*IDepartmentRepository repository*/ IUnitOfWork unitOfWork, IWebHostEnvironment environment,IMapper mapper)
        {
            //after use UnitOfWork
            _unitOfWork = unitOfWork;

            //in GenericRepository before use UnitOfWork
            //_repository = repository;

            _environment = environment;
            _mapper = mapper;
        }

        //[HttpGet] is the default
        public IActionResult Index(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    //var departmentt = _repository.GetAll();
                    var departmentt = _unitOfWork.DepartmentRepository.GetAll();
                    var mapped= _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departmentt);
                    return View(mapped);
                }
                //var department = _repository.GetByName(Name);
                var department = _unitOfWork.DepartmentRepository.GetByName(Name);
                var mapp=_mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(department);
                return View(mapp);
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    //log exception (the developer to handle the exception)
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    //friendly message to user
                    ModelState.AddModelError(string.Empty, "An Error occured during add department");
                }
                return View();
            }

        }

        //[HttpGet] the default 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//to prevent any tool to arrive to this end-point(action)and edit any value of prop. of obj (parameter)=>(browser user only can)
                                  //above any action deal with DB and take parameter
        public IActionResult Create(DepartmentViewModel newDepartmentVM)
        {
            if (ModelState.IsValid)//mean that all validations (sever side or client side(check after send request to server and response by the check result)) is ok
            {
                var mapped= _mapper.Map<DepartmentViewModel,Department>(newDepartmentVM);
                //var count = _repository.Add(mapped);
                _unitOfWork.DepartmentRepository.Add(mapped);
                var count = _unitOfWork.Save();
                if (count > 0)//to handle any exception appear in DB(to catch if doesn't add)
                {
                    TempData["Message"] = "successfully created";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "not successfully created";
                }
            }
            return View(newDepartmentVM);//the reason of give the newDepartment as value of parameter that if Modelstate is not valid it will be in the same page with same data

        }

        //[HttpGet] the default 
        public IActionResult Details(int? id, string ViewName = "Details")//make int nullable to catch if the front not send the id value 
        {
            if (!id.HasValue)//if front doesnot send id (forget write asp-route-id="..")
            {
                return BadRequest();//400 state-Code (if front send wrong data or doesnot send any data ) 
            }
            //Department dep = _repository.GetById(id.Value);
            Department dep = _unitOfWork.DepartmentRepository.GetById(id.Value);
            var mapped = _mapper.Map<Department,DepartmentViewModel>(dep);
            if (dep == null)//if data not found in DB
            {
                return NotFound();// 404 state_Code (if the data not found in DB)
            }
            return View(ViewName, mapped);
        }

        //[HttpGet] the default 
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");//to prevent code repeating
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//to prevent any tool to arrive to this end-point(action)and edit any value of prop. of obj (parameter)=>(browser user only can)
                                  //above any action deal with DB and take parameter
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)// I make id take its value from Route only to prevent any one to edit the id value from f12 code (in website) or any tools
        {
            if (ModelState.IsValid)//mean that all validations (sever side or client side(check after send request to server and response by the check result)) is ok
            {
                if (id != departmentVM.Id)//if any tool edit the department.Id then it will send badRequest
                {
                    return BadRequest();
                }
                try//to handle any exception appear in DB
                {
                    var mapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    //_repository.Update(mapped);
                    _unitOfWork.DepartmentRepository.Update(mapped);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        //log exception (the developer to handle the exception)
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    else
                    {
                        //friendly message to user
                        ModelState.AddModelError(string.Empty, "An Error occured during edit department");
                    }
                }
            }
            return View(departmentVM);//the reason of give the newDepartment as value of parameter that if Modelstate is not valid it will be in the same page with same data

        }

        //[HttpGet] the default
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");//to prevent code repeating
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//to prevent any tool to arrive to this end-point(action) and edit any value of prop. of obj (parameter)=>(browser user only can)
                                  //above any action deal with DB and take parameter
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try//to handle any exception appear in DB
            {
                var mapped = _mapper.Map<DepartmentViewModel,Department>(departmentVM);
                //_repository.Delete(mapped);
                _unitOfWork.DepartmentRepository.Delete(mapped);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    //log exception (the developer to handle the exception)
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    //friendly message to user
                    ModelState.AddModelError(string.Empty, "An Error occured during edit department");
                }
                return View(departmentVM);//the reason of give the newDepartment as value of parameter that if Modelstate is not valid it will be in the same page with same data
            }
        }
    }
}