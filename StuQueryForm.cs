/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public class StuQueryForm
    {
    }
}*/

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
using Microsoft.Bot.Builder.Azure;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace SimpleEchoBot
{
    [Serializable]
    public class StuQueryForm
    {
        [Prompt("Please Enter your {&}")]
        public string StudentID { get; set; }
        // public string courseID { get; set; }

        public static IForm<StuQueryForm> BuildForm()
        {
            return new FormBuilder<StuQueryForm>()
                .Field(nameof(StudentID))
                .Confirm("Your ID \r :{StudentID}\r Are you Sure?")
                .Build();
        }
    }
}