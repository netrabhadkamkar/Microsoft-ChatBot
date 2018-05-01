/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public class ProfQueryForm
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
    public class ProfQueryForm
    {
        [Prompt("Please Enter your {&}")]
        public string ID { get; set; }

        [Prompt("Please Enter your {&}")]
        public string courseID { get; set; }
        public string studentID { get; set; }

        public static IForm<ProfQueryForm> BuildForm()
        {
            return new FormBuilder<ProfQueryForm>()
                .Field(nameof(ID))
                .Field(nameof(courseID))
                .Confirm("Your ID \r :{ID}\n\n Course ID: {courseID}\r Are you Sure?")
                .Build();
        }


    }

}