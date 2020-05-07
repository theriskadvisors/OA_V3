using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SEA_Application.Models;

namespace SEA_Application.Controllers
{
    public class AspnetQuestionsController : Controller
    {
        private SEA_DatabaseEntities db = new SEA_DatabaseEntities();

        // GET: AspnetQuestions
        public ActionResult Index()
        {
            var aspnetQuestions = db.AspnetQuestions.Include(a => a.AspnetLesson).Include(a => a.AspnetOption);
            return View(aspnetQuestions.ToList());
        }
        public ActionResult ViewQuestionAndQuiz()
        {
            var aspnetQuestions = db.AspnetQuestions.Include(a => a.AspnetLesson).Include(a => a.AspnetOption);
            return View(aspnetQuestions.ToList());
        }
        public ActionResult AllQuizList()
        {

            var AllLessons = (from Quiz in db.AspnetQuizs
                              select new
                              {
                                  QuizId = Quiz.Id,
                                  QuizName = Quiz.Name,
                                  QuizDescription = Quiz.Description,
                                  QuizStartDate = Quiz.Start_Date,
                                  QuizDueDate = Quiz.Due_Date,
                                  QuizCreatedBy = Quiz.Created_By,
                                  QuizCreationDate = Quiz.CreationDate,

                              }).ToList();

            return Json(AllLessons, JsonRequestBehavior.AllowGet);
        }


        // GET: AspnetQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetQuestion aspnetQuestion = db.AspnetQuestions.Find(id);
            if (aspnetQuestion == null)
            {
                return HttpNotFound();
            }
            return View(aspnetQuestion);
        }

        // GET: AspnetQuestions/Create
        public ActionResult Create()
        {
            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");
            ViewBag.TeacherID = new SelectList(db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Teacher")), "Id", "Name");

            return View();
        }

        // POST: AspnetQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionAnswerViewModel QuestionAnswerViewModel)
        {





            var id = User.Identity.GetUserId();
            var username = db.AspNetUsers.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            AspnetQuestion Question = new AspnetQuestion();
            Question.Name = QuestionAnswerViewModel.QuestionName;

            /// string IsMandatory = Request.Form["IsMandatory"];
            //if (IsMandatory == "on")
            //{
            //    Question.Is_Active = true;

            //}
            //else
            //{
            //    Question.Is_Active = false;
            //}

            // Question.Is_Active = QuestionAnswerViewModel.QuestionIsActive;
            Question.Is_Active = QuestionAnswerViewModel.QuestionIsActive;

            Question.Is_Quiz = QuestionAnswerViewModel.QuestionIsQuiz;
            Question.Type = QuestionAnswerViewModel.QuestionType;
            Question.LessonId = QuestionAnswerViewModel.LessonId;
            Question.AnswerId = null;
            Question.CreatedBy = username;
            Question.CreationDate = DateTime.Now;
            db.AspnetQuestions.Add(Question);
            db.SaveChanges();
            var QuestionType = QuestionAnswerViewModel.QuestionType;
            if (QuestionType == "MCQ" || QuestionType == "TF")
            {
                AspnetOption Op1 = new AspnetOption();
                Op1.Name = QuestionAnswerViewModel.OptionNameOne;
                Op1.QuestionId = Question.Id;
                Op1.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op1);
                db.SaveChanges();
                AspnetOption Op2 = new AspnetOption();
                Op2.Name = QuestionAnswerViewModel.QuestionNameTwo;
                Op2.QuestionId = Question.Id;
                Op2.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op2);
                db.SaveChanges();

                AspnetOption Op3 = new AspnetOption();
                Op3.Name = QuestionAnswerViewModel.QuestionNameThree;
                Op3.QuestionId = Question.Id;
                Op3.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op3);
                db.SaveChanges();

                AspnetOption Op4 = new AspnetOption();
                Op4.Name = QuestionAnswerViewModel.QuesitonNameFour;
                Op4.QuestionId = Question.Id;
                Op4.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op4);
                db.SaveChanges();

                int AnswerId;

                if (QuestionAnswerViewModel.Answer == "a")
                {

                    AnswerId = Op1.Id;
                }

                else if (QuestionAnswerViewModel.Answer == "b")
                {
                    AnswerId = Op2.Id;

                }

                else if (QuestionAnswerViewModel.Answer == "c")
                {
                    AnswerId = Op3.Id;

                }

                else

                {
                    AnswerId = Op4.Id;

                }

                Question.AnswerId = AnswerId;
                db.SaveChanges();

            }
            else
            {
                AspnetOption Op = new AspnetOption();
                Op.Name = QuestionAnswerViewModel.FillAnswer;
                Op.QuestionId = Question.Id;
                Op.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op);
                db.SaveChanges();
                int AnswerId;
                AnswerId = Op.Id;
                Question.AnswerId = AnswerId;
                db.SaveChanges();
            }


            ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name");
            return RedirectToAction("ViewQuestionAndQuiz");


        }
        public ActionResult TestQuestions()
        {
            return View();
        }

        public ActionResult TestQuestionsByTopics(int bdoIds)
        {

            //List<AspnetSubjectTopic> AllTopics = (from topic in db.AspnetSubjectTopics
            //                                      where bdoIds.Contains(topic.Id)
            //                                      select topic).ToList();

            //var AllQuestions =   AllTopics.Select(x => x.AspnetLessons.Select(y => y.AspnetQuestions).Select(y => y)).ToList();


            var AllQuestion = (from topic in db.AspnetSubjectTopics
                               join lesson in db.AspnetLessons on topic.Id equals lesson.TopicId
                               join question in db.AspnetQuestions on lesson.Id equals question.LessonId

                               where topic.Id == bdoIds && question.Is_Quiz == false && question.Type == "MCQ"
                               select new
                               {
                                   question.Id,
                                   question.Name

                               }).ToList();





            return Json(AllQuestion, JsonRequestBehavior.AllowGet);
        }

        // GET: AspnetQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetQuestion aspnetQuestion = db.AspnetQuestions.Find(id);

            QuestionAnswerViewModel QuestionViewModel = new QuestionAnswerViewModel();

            if (aspnetQuestion != null)
            {
                QuestionViewModel.QuestionIsQuiz = Convert.ToBoolean(aspnetQuestion.Is_Quiz);
                QuestionViewModel.QuestionType = aspnetQuestion.Type;
                QuestionViewModel.QuestionName = aspnetQuestion.Name;
                QuestionViewModel.Id = aspnetQuestion.Id;
                QuestionViewModel.QuestionIsActive = Convert.ToBoolean(aspnetQuestion.Is_Active);
                string IsMandatory;


                //if(QuestionViewModel.QuestionIsActive  ==true)
                //{

                //    IsMandatory = "on";
                //}
                //else
                //{
                //    IsMandatory = "";

                //}

                string[] options = db.AspnetOptions.Where(x => x.QuestionId == aspnetQuestion.Id).Select(x => x.Name).ToArray();

                QuestionViewModel.OptionNameOne = options[0];
                QuestionViewModel.QuestionNameTwo = options[1];
                QuestionViewModel.QuestionNameThree = options[2];
                QuestionViewModel.QuesitonNameFour = options[3];

                List<int> AnwersListToChoones = db.AspnetOptions.Where(x => x.QuestionId == aspnetQuestion.Id).Select(x => x.Id).ToList();

                int count = 1;
                foreach (int FindAsnwer in AnwersListToChoones)
                {


                    if (FindAsnwer == aspnetQuestion.AnswerId)
                    {

                        break;

                    }
                    count++;

                }
                string Answer = "";
                if (count == 1)
                {
                    Answer = "a";
                }
                else if (count == 2)
                {
                    Answer = "b";

                }
                else if (count == 3)
                {
                    Answer = "c";

                }
                else if (count == 4)
                {


                    Answer = "d";

                }
                else
                {
                    Answer = "";

                }
                ViewBag.Answer = Answer;

                //     ViewBag.IsMandatory = IsMandatory;
                int? TopicId = db.AspnetLessons.Where(x => x.Id == aspnetQuestion.LessonId).FirstOrDefault().TopicId;

                int? SubjectId = db.AspnetSubjectTopics.Where(x => x.Id == TopicId).FirstOrDefault().SubjectId;

                string SubjectType = db.GenericSubjects.Where(x => x.Id == SubjectId).FirstOrDefault().SubjectType;

                //   ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == SubjectType), "Id", "SubjectName", SubjectId);


                ViewBag.SubId = new SelectList(db.GenericSubjects.Where(x => x.SubjectType == SubjectType), "Id", "SubjectName", SubjectId);
                ViewBag.TopicId = new SelectList(db.AspnetSubjectTopics.Where(x => x.SubjectId == SubjectId), "Id", "Name", TopicId);
                ViewBag.LessonId = new SelectList(db.AspnetLessons.Where(x => x.TopicId == TopicId), "Id", "Name", aspnetQuestion.LessonId);
                ViewBag.CTId = SubjectType;


            }



            //  ViewBag.Aswer = aspnetQuestion



            //ViewBag.AnswerId = new SelectList(db.AspnetOptions, "Id", "Name", aspnetQuestion.AnswerId);



            return View(QuestionViewModel);
        }

        // POST: AspnetQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAnswerViewModel QuestionAnswerViewModel)
        {

            AspnetQuestion Question = db.AspnetQuestions.Where(x => x.Id == QuestionAnswerViewModel.Id).FirstOrDefault();


            Question.Name = QuestionAnswerViewModel.QuestionName;
            Question.LessonId = QuestionAnswerViewModel.LessonId;
            Question.Is_Quiz = QuestionAnswerViewModel.QuestionIsQuiz;


            //string IsMandatory = Request.Form["IsMandatory"];
            //if (IsMandatory == "on")
            //{
            //    QuestionAnswerViewModel.QuestionIsActive = true;

            //}
            //else
            //{
            //    QuestionAnswerViewModel.QuestionIsActive = false;
            //}

            // Question.Is_Active = QuestionAnswerViewModel.QuestionIsActive;


            db.SaveChanges();

            var QuestionType = QuestionAnswerViewModel.QuestionType;
            if (QuestionType == "MCQ" || QuestionType == "TF")
            {
                AspnetOption[] options = db.AspnetOptions.Where(x => x.QuestionId == QuestionAnswerViewModel.Id).ToArray();

                options[0].Name = QuestionAnswerViewModel.OptionNameOne;
                options[1].Name = QuestionAnswerViewModel.QuestionNameTwo;
                options[2].Name = QuestionAnswerViewModel.QuestionNameThree;
                options[3].Name = QuestionAnswerViewModel.QuesitonNameFour;
                db.SaveChanges();


                int AnswerId;

                if (QuestionAnswerViewModel.Answer == "a")
                {

                    AnswerId = options[0].Id;
                }

                else if (QuestionAnswerViewModel.Answer == "b")
                {
                    AnswerId = options[1].Id;

                }

                else if (QuestionAnswerViewModel.Answer == "c")
                {
                    AnswerId = options[2].Id;

                }

                else

                {
                    AnswerId = options[3].Id;

                }

                Question.AnswerId = AnswerId;
                db.SaveChanges();

            }
            else
            {
                AspnetOption Op = new AspnetOption();
                Op.Name = QuestionAnswerViewModel.FillAnswer;
                Op.QuestionId = Question.Id;
                Op.CreationDate = DateTime.Now;
                db.AspnetOptions.Add(Op);
                db.SaveChanges();
                int AnswerId;
                AnswerId = Op.Id;
                Question.AnswerId = AnswerId;
                db.SaveChanges();
            }

            //    if (ModelState.IsValid)
            //    {
            //        db.Entry(aspnetQuestion).State = EntityState.Modified;
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    ViewBag.LessonId = new SelectList(db.AspnetLessons, "Id", "Name", aspnetQuestion.LessonId);
            //    ViewBag.AnswerId = new SelectList(db.AspnetOptions, "Id", "Name", aspnetQuestion.AnswerId);
            //    return View(aspnetQuestion);

            return RedirectToAction("ViewQuestionAndQuiz");
        }

        // GET: AspnetQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspnetQuestion aspnetQuestion = db.AspnetQuestions.Find(id);
            if (aspnetQuestion == null)
            {
                return HttpNotFound();
            }
            return View(aspnetQuestion);
        }

        // POST: AspnetQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspnetQuestion aspnetQuestion = db.AspnetQuestions.Find(id);
            db.AspnetQuestions.Remove(aspnetQuestion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
