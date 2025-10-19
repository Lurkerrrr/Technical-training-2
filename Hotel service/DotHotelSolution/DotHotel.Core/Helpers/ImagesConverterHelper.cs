namespace DotHotel.Core.Helpers
{
    public static class ImagesConverterHelper
    {
        public static List<byte[]> ConvertBase64ImagesToByteArrays(List<string> base64Images)
        {
            var result = new List<byte[]>();

            if (base64Images == null || !base64Images.Any())
                return result;

            foreach (var base64String in base64Images)
            {
                if (string.IsNullOrWhiteSpace(base64String))
                    continue;

                string base64Data;

                if (base64String.StartsWith("data:"))
                {
                    var base64Index = base64String.IndexOf("base64,");
                    if (base64Index != -1)
                    {
                        base64Data = base64String.Substring(base64Index + 7);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    base64Data = base64String;
                }

                try
                {
                    var imageBytes = Convert.FromBase64String(base64Data);
                    result.Add(imageBytes);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public static List<string> ConvertByteArraysToBase64Images(List<byte[]> imagesBytes, string mimeType = "image/gif")
        {
            var result = new List<string>();

            if (imagesBytes == null || !imagesBytes.Any())
                return result;

            foreach (var imageBytes in imagesBytes)
            {
                var base64String = Convert.ToBase64String(imageBytes);
                result.Add($"data:{mimeType};base64,{base64String}");
            }

            return result;
        }
    }

}
