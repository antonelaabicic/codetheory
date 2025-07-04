using codetheory.BL.Models;
using codetheory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace codetheory.BL.Validation
{
    public static class LessonContentValidator
    {
        public static object ParseAndValidateContentData(LessonContent content)
        {
            try
            {
                switch (content.ContentTypeId)
                {
                    case 1:
                        using (JsonDocument doc = JsonDocument.Parse(content.ContentData))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("question", out _) && root.TryGetProperty("answer", out _))
                            {
                                var joke = JsonSerializer.Deserialize<JokeContentData>(content.ContentData);

                                if (joke == null || string.IsNullOrWhiteSpace(joke.Question) || string.IsNullOrWhiteSpace(joke.Answer))
                                {
                                    throw new Exception("Joke content is missing question or answer.");
                                }

                                return joke!;
                            }

                            var text = JsonSerializer.Deserialize<TextContentData>(content.ContentData);
                            if (text == null || string.IsNullOrWhiteSpace(text.Title) || string.IsNullOrWhiteSpace(text.Text))
                            {
                                throw new Exception("Text content is missing title or text.");
                            }

                            return text!;
                        }

                    case 2: 
                        var image = JsonSerializer.Deserialize<ImageContentData>(content.ContentData);
                        if (image == null || string.IsNullOrWhiteSpace(image.ImagePath))
                        {
                            throw new Exception("Image path is required.");
                        }
                        return image;

                    case 3: 
                        var video = JsonSerializer.Deserialize<VideoContentData>(content.ContentData);
                        if (video == null || string.IsNullOrWhiteSpace(video.Url))
                        {
                            throw new Exception("Video URL is required.");
                        }
                        return video;

                    case 4:
                        var code = JsonSerializer.Deserialize<CodeContentData>(content.ContentData);
                        if (code == null || string.IsNullOrWhiteSpace(code.Code) || string.IsNullOrWhiteSpace(code.Language))
                        {
                            throw new Exception("Code content is incomplete.");
                        }
                        return code;

                    default:
                        throw new NotSupportedException("Unknown content type.");
                }
            }
            catch (JsonException ex)
            {
                throw new Exception("Invalid JSON format.", ex);
            }
        }
    }
}
