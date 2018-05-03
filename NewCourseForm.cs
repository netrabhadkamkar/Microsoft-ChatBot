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
using System.Runtime.Remoting.Contexts;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace SimpleEchoBot
{
    [Serializable]
    public class NewCourseForm
    {
        [Prompt("Please Enter your {&}")]
        public string ID { get; set; }

        [Prompt ("Please Enter Course Name {&}")]
        public string Courses { get; set; }

        [Prompt("Please Enter your {&}")]
        public string courseID { get; set; }
        public string studentID { get; set; }

        public static IForm<NewCourseForm> BuildForm()
        {
            return new FormBuilder<NewCourseForm>()
                .Field(nameof(ID))
                .Field(nameof(Courses))
                .Field(nameof(courseID))
                .Confirm("Your ID \r :{ID}\n\n \n\nCourse :{Courses} Course ID: {courseID}\r Are you Sure?")
                .Build();
        }
    }
}