using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Repository;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.Helpers;
using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        //same comments as DepartmentController

        //after use UnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        //in GenericRepository before use UnitOfWork
        //private readonly IEmployeeRepository _employeeRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public EmployeeController(/*IEmployeeRepository employeeRepository*/ IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment,IMapper mapper) 
        {
            _unitOfWork = unitOfWork;

            //in GenericRepository before use UnitOfWork
            //_employeeRepository = employeeRepository;

            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public IActionResult Index(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    //var employees = _employeeRepository.GetAll();
                    var employees = _unitOfWork.EmployeeRepository.GetAll();
                    var mapped = _mapper.Map< IEnumerable<Employee> , IEnumerable<EmployeeViewModel> >(employees);
                    return View(mapped);
                }
                //var emp = _employeeRepository.GetByName(Name);
                var emp = _unitOfWork.EmployeeRepository.GetByName(Name);
                var mappedemp=_mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(emp);
                return View(mappedemp);
            }
            catch (Exception ex)
            {
                if (_webHostEnvironment.IsDevelopment())
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
        public IActionResult Create()
        {
            ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();//after use UnitOfWork
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM) 
        {

            if (ModelState.IsValid)
            {
                if (employeeVM.Image != null) //because it will throw exception if user send empty file to Upload
                {
                    
                    employeeVM.ImageName = UploadFile.Upload(employeeVM.Image, "Images");//to upload the image in the Images folder(in my code) and return the fileName
                                                                                         // because ImageName comed from view = null (mlhash input)
                }
                var mapped= _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                //var count = _employeeRepository.Add(mapped);
                _unitOfWork.EmployeeRepository.Add(mapped);
                var count = _unitOfWork.Save();
                if (count > 0)
                {
                    TempData["Message"] = "successfully created";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "not successfully created";
                }
            }
            ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();//after use UnitOfWork
            return View(employeeVM);
        }
        public IActionResult Details(int? id,string ViewName="Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            //var employee= _employeeRepository.GetById(id.Value);
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            var mapped= _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee == null)
            {
                return NotFound();
            }
            if(ViewName== "Edit")//after use UnitOfWork
            {
                ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();
                return View(ViewName, mapped);
            }
            return View(ViewName,mapped);
        }
        public IActionResult Delete(int id) 
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employeeVM)
        {
            try
            {
               
                var mapped = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                //_employeeRepository.Delete(mapped);
                _unitOfWork.EmployeeRepository.Delete(mapped);
                _unitOfWork.Save();
                if (employeeVM.ImageName != null)  //because it will throw exception if user send empty fileName to Delete 
                {
                    UploadFile.Delete(employeeVM.ImageName, "Images");//to delete the image from Images folder(in my code) after the delete from DB
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message); 
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "error in delete");
                }
                return View(employeeVM);
            }
        }
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,EmployeeViewModel employeeVM)
        {
            if(ModelState.IsValid) {
                if (employeeVM.Id != id)
                { 
                return BadRequest();
                }
                try
                {
                    if (employeeVM.Image != null) //because it will throw exception if user send empty file to Upload and if user send empty fileName to Delete 
                    {
                        var name = employeeVM.ImageName; // to keep the old ImageName
                        employeeVM.ImageName = UploadFile.Upload(employeeVM.Image, "Images");//to upload the image in the Images folder(in my code) and return the fileName
                        UploadFile.Delete(name, "Images");//to delete the old Image from Images folder(in my code)
                    }
                    var mapped = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                    //_employeeRepository.Update(mapped);
                    _unitOfWork.EmployeeRepository.Update(mapped);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (_webHostEnvironment.IsDevelopment())
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "error in delete");
                    }
                }
            }
            ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();//after use UnitOfWork
            return View(employeeVM);
        }
    }
}
