using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Azure;
using System.Runtime.Remoting.Contexts;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SimpleEchoBot.Dialogs
{
    [LuisModel("aa0a6d2f-4982-4f9a-9c55-52ccdd97907b", "26143a420dea44d59ddd0ed8570d5044")]
    [Serializable]
    public class Professor : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]

        public async Task None(IDialogContext context, LuisResult result)
        {
            //string message = $"Sorry, I don't understand '{result.Query}'";
            string message = $"Sorry, I don't understand '{result.Query}'";
            await context.PostAsync(message);
            // context.Wait(MessageReceived);
            context.Wait(MessageReceived);
        }
        [LuisIntent("Greetings")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {

            await context.PostAsync($"Welcome ! Are you a Professor or Student?");
            context.Wait(MessageReceived);

        }
        [LuisIntent("Professor")]
        public async Task ProfessorSearch(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I need you to answer few Questions!");
            var form = new FormDialog<ProfQueryForm>(new ProfQueryForm(), ProfQueryForm.BuildForm, FormOptions.PromptInStart);
            context.Call<ProfQueryForm>(form, ProfQueryComplete);

        }
        private async Task ProfQueryComplete(IDialogContext context, IAwaitable<ProfQueryForm> result)
        {
            ProfQueryForm form = null;
            try
            {
                form = await result;

            }
            catch (OperationCanceledException)
            {

            }
            if (form == null)
            {
                await context.PostAsync("You Canceled the form.");

            }

            if (form != null)
            {

                List<Teacher> teachers = Database.GetTeachers(form.ID, form.courseID);
                List<Student1> stu = Database.GetStu(form.ID, form.courseID);
               

                if (teachers == null || teachers.Count == 0)
                {
                    await context.PostAsync($"Sorry, could not find Courses for ID {form.ID} ");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append($"Hi There! I was able to find **{teachers.Count.ToString()}** Course(s) \n\n");
                sb.Append("--- \n\n");
                // int s = 0;
                foreach (Teacher teacher in teachers)
                {
                    //s += 1;
                    sb.Append($"* Professor id is **{teacher.ID}** ; CourseID is **{teacher.CourseID}** ; Course is **{teacher.Courses}**  \n\n");
                    // sb.Append($"* Students enrolled under {teacher.ID} are ID {teacher.CourseID}\n\n");
                    //sb.Append($"")
                    //sb.Append($"Total cost will be {flight.fare}+ {FlightsQueryForm.cost}");
                    //  sb.Append(s);
                }

                sb.Append($"There are **{stu.Count.ToString()}** Students Enrolled\n\n");

                foreach (Student1 std in stu)
                {
                    //sb.Append($"*Students enrolled under {std.CourseID} are ID {std.StudentID}");
                    sb.Append($"*Students enrolled under {std.CourseID} are ID {std.StudentID}\n\n");

                }

                var message = $"Looking for courses  {form.ID} ";
                await context.PostAsync(sb.ToString());



            }

            context.Wait(MessageReceived);
        }


        [LuisIntent("Student")]
        public async Task StudentSearch(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I need you to answer few Questions!");
            var form = new FormDialog<StuQueryForm>(new StuQueryForm(), StuQueryForm.BuildForm, FormOptions.PromptInStart);
            context.Call<StuQueryForm>(form, StuQueryComplete);

        }
        private async Task StuQueryComplete(IDialogContext context, IAwaitable<StuQueryForm> result)
        {
            StuQueryForm form = null;
            try
            {
                form = await result;

            }
            catch (OperationCanceledException)
            {

            }
            if (form == null)
            {
                await context.PostAsync("You Canceled the form.");

            }

            if (form != null)
            {
                List<Student> students = Database1.GetStudents(form.StudentID);

                if (students == null || students.Count == 0)
                {
                    await context.PostAsync($"Sorry, could not find Students for ID {form.StudentID} ");
                    return;
                }

                StringBuilder sb1 = new StringBuilder();
                sb1.Append($"Hi There! I was able to find **{students.Count.ToString()}** Course(s) \n\n");
                sb1.Append("--- \n\n");
                foreach (Student student in students)
                {
                    // sb.Append($"You are enrolled in **{student.courseID}**\n\n");
                    sb1.Append($"* Student id is **{student.StudentID}** ; Course is **{student.courseID}**  \n\n");
                }



                var message = $"Looking for courses  {form.StudentID} ";

                //  var message1 = $"Looking for students {form.courseID}";
                await context.PostAsync(sb1.ToString());
            }

            context.Wait(MessageReceived);
        }
        [LuisIntent("Courses")]
         public async Task CourseSearch(IDialogContext context, LuisResult result)
         {

             await context.PostAsync("I need you to answer few Questions!");
             var form = new FormDialog<CouQueryForm>(new CouQueryForm(), CouQueryForm.BuildForm, FormOptions.PromptInStart);
             context.Call<CouQueryForm>(form, CouQueryComplete);

         }

        private async Task CouQueryComplete(IDialogContext context, IAwaitable<CouQueryForm> result)
        {
            CouQueryForm form = null;
            try
            {
                form = await result;
            }
            catch(OperationCanceledException)
            {

            }
            if(form != null)
            {
                List<Course> courses = Database2.GetCourse(form.CourseName);
                 if(courses == null || courses.Count ==0)
                {
                    await context.PostAsync($"Sorry, could not find Course for {form.CourseName} ");
                    return;
                }

                StringBuilder sb2 = new StringBuilder();
                sb2.Append($"Hi There! I was able to find **{courses.Count.ToString()}** Courses \n\n");
                sb2.Append("--\n\n");
                foreach(Course course in courses)
                {
                    sb2.Append($"Course ID is **{course.CourseID}**;\n Course Name is **{course.CourseName}**;\n Course Department is **{course.Department}**; \n Course Level is **{course.Level}**;\n Course Intake is **{course.Intake}**; \n Course will have **{course.Assignments}** Assignments; \n Course is divided in following way \n Attendance **{course.Attendance}** %; \n Class Participation **{course.CP}** %; \n Exam 1 **{course.Exam1}** %; \n Exam2 **{course.Exam2}** %; \n Project **{course.Project}**; \n HomeWork **{course.HW}**%");

                }

                var message = $"Looking for courses  {form.CourseName} ";

                //  var message1 = $"Looking for students {form.courseID}";
                await context.PostAsync(sb2.ToString());


            }
            context.Wait(MessageReceived);

        }
        [LuisIntent("NewEntry")]
        public async Task NewEntry(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I need you to answer few Questions!");
            var form = new FormDialog<NewCourseForm>(new NewCourseForm(), NewCourseForm.BuildForm, FormOptions.PromptInStart);
            context.Call<NewCourseForm>(form, NewCourseComplete);

        }
        private async Task NewCourseComplete(IDialogContext context, IAwaitable<NewCourseForm> result)
        {
            NewCourseForm form = null;
            try
            {
                form = await result;
            }
            catch (OperationCanceledException)
            {

            }
            if (form != null)
            {
                List<Subject> subjects = Database3.InsertCourse(form.ID, form.Courses, form.courseID);
               /* if (subjects == null || subjects.Count == 0)
                {
                    await context.PostAsync($"Sorry, could not find Course for {form.Courses} ");
                    return;
                }*/

                StringBuilder sb3 = new StringBuilder();
                sb3.Append($"Hi There! I was able to insert Course(s) \n\n");
                //sb3.Append("--\n\n");
                foreach (Subject subject in subjects)
                {
                    sb3.Append($"Course ID is **{subject.ID}**;\n Course Name is **{subject.Course}**;\n Course ID is **{subject.CourseID}** ");

                }

                var message = $"Looking for courses  {form.Courses} ";

                //  var message1 = $"Looking for students {form.courseID}";
                await context.PostAsync(sb3.ToString());


            }
            context.Wait(MessageReceived);

        }

        [LuisIntent("SetAppoint")]
        public async Task Appointment(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I need you to answer few Questions!");
            var form = new FormDialog<AppointForm>(new AppointForm(), AppointForm.BuildForm, FormOptions.PromptInStart);
            context.Call<AppointForm>(form, AppointComplete);

        }
        private async Task AppointComplete(IDialogContext context, IAwaitable<AppointForm> result)
        {
            AppointForm form = null;
            try
            {
                form = await result;
            }
            catch (OperationCanceledException)
            {

            }
            if (form != null)
            {
                List<Appoint> meets = Database4.SetAppoint(form.ProfessorID);
                /* if (meets == null || meets.Count == 0)
                 {
                     await context.PostAsync($"Sorry, could not find Time for {form.ProfessorID} ");
                     return;
                 }*/

                StringBuilder sb4 = new StringBuilder();
                sb4.Append($"Hi There! I was able to find slots \n\n");
                sb4.Append("--\n\n");
                foreach (Appoint app in meets)
                {
                    sb4.Append($"Professor ID is **{app.ID}** and Timings are **{app.Time}**\n\n\n");

                }

                var message = $"Looking for courses  {form.ProfessorID} ";

                //  var message1 = $"Looking for students {form.courseID}";
                await context.PostAsync(sb4.ToString());


            }
            context.Wait(MessageReceived);

        }
        [LuisIntent("ConfirmAppoint")]
        public async Task ConfirmAppt(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I need you to answer few Questions!");
            var form = new FormDialog<ConfirmForm>(new ConfirmForm(), ConfirmForm.BuildForm, FormOptions.PromptInStart);
            context.Call<ConfirmForm>(form, ConfirmComplete);

        }
        private async Task ConfirmComplete(IDialogContext context, IAwaitable<ConfirmForm> result)
        {
            ConfirmForm form = null;
            try
            {
                form = await result;
            }
            catch (OperationCanceledException)
            {

            }
            if (form != null)
            {
                List<Confirm> appoints = Database5.ConfirmAppoint(form.Time, form.ProfessorID);
                /* if (meets == null || meets.Count == 0)
                 {
                     await context.PostAsync($"Sorry, could not find Time for {form.ProfessorID} ");
                     return;
                 }*/

                StringBuilder sb4 = new StringBuilder();
                sb4.Append($"Hi There! I was able to fix your appointment \n\n");
                sb4.Append("--\n\n");
                foreach (Confirm app in appoints)
                {
                    sb4.Append($"Professor ID is **{app.ID}** and Timings are **{app.Time}**\n ");

                }

                var message = $"Looking for courses  {form.ProfessorID} ";

                //  var message1 = $"Looking for students {form.courseID}";
                await context.PostAsync(sb4.ToString());


            }
            context.Wait(MessageReceived);

        }

        public class Teacher
        {

            public string ID { get; set; }
            public string Courses { get; set; }
            public string CourseID { get; set; }
        }

        public class Student1
        {
            public string CourseID { get; set; }
            public string StudentID { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public string Stream { get; set; }

        }

        public class Student
        {
            public string StudentID { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public string Stream { get; set; }
            public string courseID { get; set; }
        }

        public class Course
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }
            public string Department { get; set; }
            public string Level { get; set; }
            public string Intake { get; set; }
            public string Assignments { get; set; }
            public string Attendance { get; set; }
            public string CP { get; set; }
            public string Exam1 { get; set; }
            public string Exam2 { get; set; }
            public string Project { get; set; }
            public string HW { get; set; }
        }

        public class Subject
        {
            public string ID { get; set; }
            public string Course { get; set; }
            public string CourseID { get; set; }

        }

        public class Appoint
        {
            //public string StudentID { get; set; }
            public string ID { get; set; }
            public string Time { get; set; }
            public string Map { get; set; }
            public string Day { get; set; }
        }

        public class Confirm
        {
            public string ID { get; set; }
            public string Time { get; set; }
            public string Map { get; set; }
            public string Day { get; set; }
        }

        class Database
        {
            public static List<Teacher> GetTeachers(string ID, string courseID)
            {
                List<Teacher> teachers = new List<Teacher>();
                //using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Database = master"))
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    //string StrQuery = $"Select * From professors Where ID = '{ID}'";
                    string StrQuery = $"select ID, Courses, courseID from professors where ID='{ID}'";
                    // string StrQuery2 = $"Select courseID From students Where courseID = CIS560";
                    SqlCommand command = new SqlCommand(StrQuery, connection);
                    //SqlCommand command1 = new SqlCommand(StrQuery2, connection1);
                    command.CommandType = CommandType.Text;
                    //command1.CommandType = CommandType.Text;
                    DataTable university = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university);
                    foreach (DataRow dr in university.Rows)
                    {

                        string id = dr["ID"].ToString();
                        string courses = dr["Courses"].ToString();
                        string courseid = dr["courseID"].ToString();
                        Teacher t = new Teacher();
                        t.ID = id;
                        t.Courses = courses;
                        t.CourseID = courseid;
                        teachers.Add(t);

                    }

                }

                return teachers;
            }
            //return teachers;
            public static List<Student1> GetStu(string ID, string courseID)
            {
                List<Student1> stu = new List<Student1>();
                using (SqlConnection connection1 = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection1.Open();
                    //string StrQuery2 = $"Select StudentID From students Where courseID = 'cis560'";
                    string StrQuery2 = $"select StudentID, courseID from students where courseID = '{courseID}'";
                    SqlCommand command1 = new SqlCommand(StrQuery2, connection1);
                    command1.CommandType = CommandType.Text;

                    DataTable university2 = new DataTable();
                    SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                    adapter1.Fill(university2);
                    foreach (DataRow dr in university2.Rows)
                    {

                        string id = dr["StudentID"].ToString();
                        // string fname = dr["FName"].ToString();
                        // string lname = dr["LName"].ToString();
                        //string stream = dr["Stream"].ToString();
                        // string courseid = dr["courseID"].ToString();
                        string courseidd = dr["courseID"].ToString();

                        Student1 t = new Student1();
                        t.StudentID = id;
                        // t.FName = fname;
                        //t.LName = lname;
                        //t.Stream = stream;
                        t.CourseID = courseidd;

                        stu.Add(t);


                    }

                }

                return stu;






            }
           

        }



        class Database1
        {

            public static List<Student> GetStudents(string studentID)
            {
                List<Student> students = new List<Student>();
                //using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Database = master"))
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    string StrQuery = $"Select * From students Where StudentID = '{studentID}'";
                    // string StrQuery = $"Select * From students Where courseID = '{studentID}'";
                    SqlCommand command = new SqlCommand(StrQuery, connection);
                    command.CommandType = CommandType.Text;
                    DataTable university2 = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university2);
                    foreach (DataRow dr in university2.Rows)
                    {

                        string id = dr["StudentID"].ToString();
                        string fname = dr["FName"].ToString();
                        string lname = dr["LName"].ToString();
                        string stream = dr["Stream"].ToString();
                        string courseid = dr["courseID"].ToString();

                        Student s = new Student();
                        s.StudentID = id;
                        s.FName = fname;
                        s.LName = lname;
                        s.Stream = stream;
                        s.courseID = courseid;

                        students.Add(s);

                    }
                }

                return students;

            }

        }


        class Database2
        {
            public static List<Course> GetCourse(string courseName)
            {
                List<Course> courses = new List<Course>();
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    string StrQuery = $"select * from courses where courseName='{courseName}'";
                    SqlCommand command = new SqlCommand(StrQuery, connection);
                    command.CommandType = CommandType.Text;
                    DataTable university3 = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university3);

                    foreach(DataRow dr in university3.Rows)
                    {
                        // string id = dr["ID"].ToString();
                        string courseid = dr["courseID"].ToString();
                        string coursename = dr["courseName"].ToString();
                        string department = dr["Department"].ToString();
                        string level = dr["level"].ToString();
                        string intake = dr["intake"].ToString();
                        string assignment = dr["Assignments"].ToString();
                        string attendance = dr["attendance"].ToString();
                        string classpart = dr["cp"].ToString();
                        string exam1 = dr["exam1"].ToString();
                        string exam2 = dr["exam2"].ToString();
                        string project = dr["project"].ToString();
                        string homework = dr["hw"].ToString();
                        // Teacher t = new Teacher();

                        Course c = new Course();
                        c.CourseID = courseid;
                        c.CourseName = coursename;
                        c.Department = department;
                        c.Level = level;
                        c.Intake = intake;
                        c.Assignments = assignment;
                        c.Attendance = attendance;
                        c.CP = classpart;
                        c.Exam1 = exam1;
                        c.Exam2 = exam2;
                        c.Project = project;
                        c.HW = homework;
                        //t.ID = id;

                        courses.Add(c);
                        
                    }
                }
                return courses;
            }
        }

        class Database3
        {
            public static List<Subject> InsertCourse(string ID, string courseName, string courseID)
            {
                List<Subject> subjects = new List<Subject>();
                //using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Database = master"))
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    //string StrQuery = $"Select * From professors Where ID = '{ID}'";
                    //string StrQuery = $"select ID, Courses, courseID from professors where ID='{ID}'";
                   // string StrQuery = $"insert into professors values (ID = '{ID}', Courses = '{courseName}', courseID = '{courseID}')";
                    string StrQuery = $"insert into professors (ID, Courses, courseID) VALUES ('{ID}','{courseName}','{courseID}')";
                    // string StrQuery2 = $"Select courseID From students Where courseID = CIS560";
                    SqlCommand command = new SqlCommand(StrQuery, connection);
                    //SqlCommand command1 = new SqlCommand(StrQuery2, connection1);
                    command.CommandType = CommandType.Text;
                    //command1.CommandType = CommandType.Text;
                    DataTable university = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university);
                    foreach (DataRow dr in university.Rows)
                    {

                        string id = dr["ID"].ToString();
                        string courses = dr["Courses"].ToString();
                        string courseid = dr["courseID"].ToString();
                        Subject t = new Subject();
                        t.ID = id;
                        t.Course = courses;
                        t.CourseID = courseid;
                        subjects.Add(t);

                    }

                }

                return subjects;
            }
        }
        
        class Database4
        {
            public static List<Appoint> SetAppoint(string ID)
            {
                List<Appoint> meets = new List<Appoint>();
                //using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Database = master"))
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    string StrQuery = $"select ID, timing from appointments where ID = '{ID}' and map = 'F' ";
                     SqlCommand command = new SqlCommand(StrQuery, connection);
                    //SqlCommand command1 = new SqlCommand(StrQuery2, connection1);
                    command.CommandType = CommandType.Text;
                    //command1.CommandType = CommandType.Text;
                    DataTable university = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university);
                    foreach (DataRow dr in university.Rows)
                    {

                        string id = dr["ID"].ToString();
                        string times = dr["timing"].ToString();
                        //string maps = dr["map"].ToString();
                        //string day = dr["day"].ToString();
                        Appoint t = new Appoint();
                        t.ID = id;
                        t.Time = times;
                        //t.Map = map;
                        //t.Day = day;
                        meets.Add(t);

                    }

                }

                return meets;
            }
        }

        class Database5
        {
            public static List<Confirm> ConfirmAppoint(string time, string ID)
            {
                List<Confirm> meets = new List<Confirm>();
                //using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Integrated Security=True;Database = master"))
                using (SqlConnection connection = new SqlConnection(@"Data Source=NETRA\SQLEXPRESS;Integrated Security=True;Database = master; uid=NETRA\Dell"))
                {
                    connection.Open();
                    //string StrQuery = $"select ID, timing from appointments where ID = '{ID}' ";
                    string StrQuery = $"update appointments set map = 'T' where timing = '{time}' and ID = '{ID}'  ";
                    //update appointments set map = 'T' where timing = '1300' and ID = '100';
                    SqlCommand command = new SqlCommand(StrQuery, connection);
                    //SqlCommand command1 = new SqlCommand(StrQuery2, connection1);
                    command.CommandType = CommandType.Text;
                    //command1.CommandType = CommandType.Text;
                    DataTable university = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(university);
                    foreach (DataRow dr in university.Rows)
                    {

                        string id = dr["ID"].ToString();
                        string times = dr["timing"].ToString();
                        string maps = dr["map"].ToString();
                        string day = dr["day"].ToString();
                        Confirm t = new Confirm();
                        t.ID = id;
                        t.Time = times;
                        t.Map = maps;
                        t.Day = day;
                        meets.Add(t);

                    }

                }

                return meets;
            }
        }


    }
}