using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeCRUD.Models;

namespace EmployeeCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        EmpCRUDDBEntities db = new EmpCRUDDBEntities();
        // GET: Employee
        public ActionResult Index()
        {
            List<EmployeeTbl> empList = db.EmployeeTbls.ToList();
            db.Dispose();
            return View(empList);
        }

        public ActionResult Create()
        {
            ViewBag.DepartmentList = new SelectList(db.DepartmentTbls, "ID", "DepartmentName");
            EmployeeTbl emp = new EmployeeTbl { JoinDate = DateTime.Now };
            return View(emp);
        }

        public ActionResult Add(EmployeeTbl emp)
        {
            emp.Active = true;
            emp.CreatedDate = DateTime.Now;
            db.EmployeeTbls.Add(emp);
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.DepartmentList = new SelectList(db.DepartmentTbls, "ID", "DepartmentName");
            EmployeeTbl empObj = db.EmployeeTbls.Where(x => x.ID == id).FirstOrDefault();
            /*db.Dispose();*/
            return View(empObj);
        }

        public ActionResult Save(EmployeeTbl emp)
        {
            EmployeeTbl empObj = db.EmployeeTbls.Where(x => x.ID == emp.ID).FirstOrDefault();
            empObj.EmployeeID = emp.EmployeeID;
            empObj.Name = emp.Name;
            empObj.Email = emp.Email;
            empObj.Phone = emp.Phone;
            empObj.DepartmentID = emp.DepartmentID;
            empObj.JoinDate = emp.JoinDate;
            empObj.Password = emp.Password;
            empObj.Active = true;
            empObj.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            db.Dispose();
            return Redirect("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeTbl empInfo)
        {
            if (ModelState.IsValid)
            {
                using(EmpCRUDDBEntities db = new EmpCRUDDBEntities())
                {
                    var obj = db.EmployeeTbls.Where(emp => emp.EmployeeID.Equals(empInfo.EmployeeID) && emp.Password.Equals(empInfo.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["ID"] = obj.ID.ToString();
                        Session["EmployeeID"] = obj.EmployeeID.ToString();
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The EmployeeID or Password Incorrect.");
                    }
                }
            }
            
            return View(empInfo);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Employee");
        }
    }
}