using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAI.Gemini
{
    internal class GeminiRequest
    {
        internal List<Content> contents { get; set; } = new();
    }

    internal class Content
    {
        internal List<Part> parts { get; set; } = new();
    }

    internal class Part
    {
        internal string text { get; set; } = string.Empty;
    }

    internal class GeminiResponse
    {
        internal List<GeminiCandidate>? Candidates { get; set; }
    }

    internal class GeminiCandidate
    {
        internal GeminiContent? Content { get; set; }
    }

    internal class GeminiContent
    {
        internal List<GeminiPart>? Parts { get; set; }
    }

    internal class GeminiPart
    {
        internal string? Text { get; set; }
    }

    public class AiGenerateResult
    {
        internal string Text { get; set; } = string.Empty;
    }
    public class AiStandardResponse<T>
    {
        internal bool Success { get; set; }
        internal T? Data { get; set; }
        internal AiMeta? Meta { get; set; }
    }
    public class AiSummaryData
    {
        internal string Summary { get; set; } = string.Empty;
        internal decimal Confidence { get; set; }
        internal List<string> Keywords { get; set; } = new();
    }

    internal class AiMeta
    {
        internal string Model { get; set; } = string.Empty;
        internal string Language { get; set; } = string.Empty;
    }
}
