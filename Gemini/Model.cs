using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectAI.Gemini
{
    public class GeminiRequest
    {
        public List<Content> contents { get; set; } = new();
    }

    public class Content
    {
        public List<Part> parts { get; set; } = new();
    }

    public class Part
    {
        public string text { get; set; } = string.Empty;
    }

    public class GeminiResponse
    {
        public List<GeminiCandidate>? Candidates { get; set; }
    }

    public class GeminiCandidate
    {
        public GeminiContent? Content { get; set; }
    }

    public class GeminiContent
    {
        public List<GeminiPart>? Parts { get; set; }
    }

    public class GeminiPart
    {
        public string? Text { get; set; }
    }

    public class AiGenerateResult
    {
        public string Text { get; set; } = string.Empty;
    }
    public class AiStandardResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public AiMeta? Meta { get; set; }
    }
    public class AiSummaryData
    {
        public string Summary { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public List<string> Keywords { get; set; } = new();
    }
    public class AiMeta
    {
        public string Model { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
