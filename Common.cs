using ConnectAI.Gemini;
using System.Text.Json;

namespace ConnectAI
{

    public static class GeminiMapper
    {
        public static AiGenerateResult ToAiGenerateResult(this string json)
        {
            var response = JsonSerializer.Deserialize<GeminiResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            var text = response?
                .Candidates?
                .FirstOrDefault()?
                .Content?
                .Parts?
                .FirstOrDefault()?
                .Text;

            return new AiGenerateResult
            {
                Text = text ?? string.Empty
            };
        }
    }

    public static class AiResponseMapper
    {
        public static AiStandardResponse<T> Parse<T>(this string json)
        {
            try
            {
                var result = JsonSerializer.Deserialize<AiStandardResponse<T>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null)
                    throw new InvalidOperationException("AI response is null");

                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"AI response is not in expected JSON format {json}", ex);
            }
        }
    }

    public static class AiSchemaGenerator
    {
        public static string GenerateJsonSchema<T>()
        {
            var schema = GenerateSchema(typeof(T));
            return JsonSerializer.Serialize(schema, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private static object GenerateSchema(Type type)
        {
            if (type == typeof(string))
                return new { type = "string" };

            if (type == typeof(bool))
                return new { type = "boolean" };

            if (type == typeof(int) || type == typeof(decimal) || type == typeof(double))
                return new { type = "number" };

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return new
                {
                    type = "array",
                    items = GenerateSchema(type.GetGenericArguments()[0])
                };
            }

            // object
            var properties = new Dictionary<string, object>();

            foreach (var prop in type.GetProperties())
            {
                properties[prop.Name] = GenerateSchema(prop.PropertyType);
            }

            return new
            {
                type = "object",
                properties,
                required = type.GetProperties().Select(p => p.Name).ToArray()
            };
        }
    }

    public static class AiPromptBuilder
    {
        public static string BuildPrompt<T>(string userRequest)
        {
            var schema = AiSchemaGenerator.GenerateJsonSchema<T>();

            return $"""
        You must respond ONLY with valid JSON.
        Do not include explanations.
        Do not use markdown.
        The JSON MUST strictly follow this schema:

        {schema}

        User request:
        {userRequest}
        """;
        }
    }

}
