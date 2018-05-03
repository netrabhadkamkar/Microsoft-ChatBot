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
    public class ConfirmForm
    {
        [Prompt("Please Enter {&}")]
        public string Time { get; set; }
        [Prompt("Please Confirm {&}")]
        public string ProfessorID { get; set; }

        public static IForm<ConfirmForm> BuildForm()
        {
            return new FormBuilder<ConfirmForm>()
                //.Field(nameof(StudentID))
                .Field(nameof(Time))
                .Field(nameof(ProfessorID))
                .Confirm("Time:{Time}\r Are you Sure?")
                .Build();
        }
    }
}