using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectAI.Enum;

namespace ConnectAI.AiPrompt
{
    internal static class AiPromptStrategyFactory
    {
        internal static IAiPromptStrategy Create(AiPromptType type)
            => type switch
            {
                AiPromptType.JsonStrict => new JsonStrictPromptStrategy(),
                AiPromptType.JsonWithConfidence => new JsonWithConfidencePromptStrategy(),
                AiPromptType.Classifier => new ClassifierPromptStrategy(),
                AiPromptType.FreeText => new FreeTextPromptStrategy(),
                _ => throw new NotSupportedException($"Prompt type '{type}' is not supported")
            };
    }

    public abstract class JsonSchemaPromptStrategyBase : IAiPromptStrategy
    {
        protected abstract string SystemInstruction { get; }
        protected abstract string OutputRules { get; }

        public virtual string BuildPrompt<T>(string userRequest)
        {
            var schema = AiSchemaGenerator.GenerateJsonSchema<T>();

            return $"""
                {SystemInstruction}

                {OutputRules}

                JSON Schema:
                {schema}

                User request:
                {userRequest}
            """;
        }
    }

    internal sealed class JsonStrictPromptStrategy : JsonSchemaPromptStrategyBase
    {
        protected override string SystemInstruction =>
            "You must respond ONLY with valid JSON.";

        protected override string OutputRules =>
            """
        - Do not include explanations
        - Do not include markdown
        - Do not include comments
        - Output must strictly match the schema
        """;
    }
    internal sealed class JsonWithConfidencePromptStrategy : JsonSchemaPromptStrategyBase
    {
        protected override string SystemInstruction =>
            "You are an AI assistant that returns structured analysis.";

        protected override string OutputRules =>
            """
        - Output JSON only
        - Include confidence score between 0 and 1
        - Follow schema strictly
        - No extra text
        """;
    }

    internal sealed class ClassifierPromptStrategy : JsonSchemaPromptStrategyBase
    {
        protected override string SystemInstruction =>
            "You are a classification engine.";

        protected override string OutputRules =>
            """
        - Choose the best matching label
        - Output JSON only
        - Do not explain your choice
        """;
    }

    internal sealed class FreeTextPromptStrategy : IAiPromptStrategy
    {
        public string BuildPrompt<T>(string userRequest)
        {
            return $"""
        Answer the following request clearly and concisely:

        {userRequest}
        """;
        }
    }

}
