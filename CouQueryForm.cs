/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public class CouQueryForm
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
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace SimpleEchoBot
{
    [Serializable]
    public class CouQueryForm
    {
        [Prompt("Please Enter {&}")]
        public string CourseName { get; set; }

        public static IForm<CouQueryForm> BuildForm()
        {
            return new FormBuilder<CouQueryForm>()
                .Field(nameof(CourseName))
                .Confirm("You Entered \r:{CourseName}\r Are you sure?")
                .Build();
        }
    }
}