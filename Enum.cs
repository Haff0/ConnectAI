using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAI
{
    public class Enum
    {
        public enum AiPromptType
        {
            JsonStrict,             // Strict JSON output with schema
            JsonWithConfidence,     // JSON + confidence score
            FreeText,               // Normal text response
            Classifier,              // Classification / label output
            News              
        }
    }
}
