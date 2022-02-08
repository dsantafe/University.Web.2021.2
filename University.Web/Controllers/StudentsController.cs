using System;
using System.Collections.Generic;
using System.Web.Mvc;
using University.BL.Models;
using University.BL.DTOs;
using University.BL.Data;
using System.Linq;
using PagedList;

namespace University.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        //  Students/Index {controller}/{action}
        [HttpGet]
        public ActionResult Index(int? studentId, int? pageSize, int? page)
        {
            #region Listar estudiantes
            //SELECT * FROM Students
            var query = context.Students.ToList();

            //var students = (from q in query
            //                where q.EnrollmentDate < DateTime.Now
            //                select new StudentDTO
            //                {
            //                    ID = q.ID,
            //                    LastName = q.LastName,
            //                    FirstMidName = q.FirstMidName,
            //                    EnrollmentDate = q.EnrollmentDate
            //                }).ToList();

            //Linq
            var students = query.Where(x => x.EnrollmentDate < DateTime.Now)
                                .Select(x => new StudentDTO
                                {
                                    ID = x.ID,
                                    LastName = x.LastName,
                                    FirstMidName = x.FirstMidName,
                                    EnrollmentDate = x.EnrollmentDate
                                }).ToList();
            #endregion

            #region Cursos asociados al estudiante
            if (studentId != null)
            {
                //SELECT r.*
                //FROM[dbo].[Enrollment] q
                //JOIN Course r ON q.CourseID = r.CourseID
                //WHERE q.StudentID = 1

                var courses = (from q in context.Enrollments
                               join r in context.Courses on q.CourseID equals r.CourseID
                               where q.StudentID == studentId
                               select new CourseDTO
                               {
                                   CourseID = r.CourseID,
                                   Title = r.Title,
                                   Credits = r.Credits
                               }).ToList();

                ViewBag.Courses = courses;
            }
            #endregion

            ViewBag.Data = "Data de prueba";
            ViewBag.Message = "Mensaje de prueba";

            //ViewData["Data"] = "Data de prueba";
            //ViewData["Message"] = "Mensaje de prueba";

            #region Paginación
            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.PageSize = pageSize;
            #endregion

            return View(students.ToPagedList(page.Value, pageSize.Value));
        }

        // Students/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentDTO student)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(student);

                if (student.EnrollmentDate > DateTime.Now)
                    throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                //INSERT INTO Students(FirstMidName,LastName,EnrollmentDate) VALUES(@FirstMidName, @LastName, @EnrollmentDate)
                context.Students.Add(new Student
                {
                    FirstMidName = student.FirstMidName,
                    LastName = student.LastName,
                    EnrollmentDate = student.EnrollmentDate
                });
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(student);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //var query = context.Students.Find(id);
            var student = context.Students.Where(x => x.ID == id)
                                          .Select(x => new StudentDTO
                                          {
                                              ID = x.ID,
                                              LastName = x.LastName,
                                              FirstMidName = x.FirstMidName,
                                              EnrollmentDate = x.EnrollmentDate
                                          }).FirstOrDefault();

            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(StudentDTO student)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(student);

                if (student.EnrollmentDate > DateTime.Now)
                    throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                //var studentModel = context.Students.Where(x => x.ID == student.ID).Select(x => x).FirstOrDefault();
                var studentModel = context.Students.FirstOrDefault(x => x.ID == student.ID);

                //campos que se van a modificar
                //sobreescribo las propiedades del modelo de base de datos
                studentModel.LastName = student.LastName;
                studentModel.FirstMidName = student.FirstMidName;
                studentModel.EnrollmentDate = student.EnrollmentDate;

                //  UPDATE Student 
                //  SET LastName = @LastName, FirstMidName = @FirstMidName, EnrollmentDate = @EnrollmentDate 
                //  WHERE ID = @ID;

                //aplique los cambios en base de datos
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(student);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!context.Enrollments.Any(x => x.StudentID == id))
            {
                var studentModel = context.Students.FirstOrDefault(x => x.ID == id);
                context.Students.Remove(studentModel);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}