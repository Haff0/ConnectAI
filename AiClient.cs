using ConnectAI.Gemini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAI.AiClient
{
    public static class AiClient
    {
        public static IAIService Gemini(string apiKey, string? model = null)
            => new GeminiAiService(apiKey, model);
    }
}
