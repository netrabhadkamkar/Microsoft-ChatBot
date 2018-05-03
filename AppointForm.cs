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
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace SimpleEchoBot
{
    [Serializable]
    public class AppointForm
    {
       // [Prompt("Please Enter your {&}")]
       // public string StudentID { get; set; }
        // public string courseID { get; set; }
        [Prompt("Please Enter {&}")]
        public string ProfessorID { get; set; }

        public static IForm<AppointForm> BuildForm()
        {
            return new FormBuilder<AppointForm>()
                //.Field(nameof(StudentID))
                .Field(nameof(ProfessorID))
                .Confirm("Professor ID:{ProfessorID}\r Are you Sure?")
                .Build();
        }
    }
}