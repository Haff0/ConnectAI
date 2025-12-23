using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAI.AiPrompt
{
    internal interface IAiPromptStrategy
    {
        string BuildPrompt<T>(string userRequest);
    }

}
