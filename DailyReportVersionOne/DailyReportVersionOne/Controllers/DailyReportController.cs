﻿using Microsoft.AspNetCore.Mvc;
using DailyReportVersionOne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DailyReportVersionOne.Controllers
{
    public class DailyReportController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public DailyReportController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateReport()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateReport(CreateReportViewModel reportModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var UserName = user.UserName;
                var CurrentDate = DateTime.Now;
                var result = await _context.Projects.FirstOrDefaultAsync(p => p.UserName == UserName && p.ProjectRecordDate.Date == DateTime.Today);
                if (result == null)
                {
                    var newProject = new Project();
                    newProject = reportModel.MyProject;
                    newProject.UserName = UserName;
                    newProject.ProjectRecordDate = CurrentDate;
                    _context.Projects.Add(newProject);
                    var newBid = new Bid();
                    newBid = reportModel.MyBid;
                    newBid.UserName = UserName;
                    newBid.BidRecordDate = CurrentDate;
                    _context.Bids.Add(newBid);
                    var newStudy = new Study();
                    newStudy = reportModel.Study;
                    newStudy.UserName = UserName;
                    newStudy.StudyRecordDate = CurrentDate;
                    _context.Studies.Add(newStudy);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMessage = $"You already reported today!!!";
                return View();
            }

            return View();
        }
    }
}