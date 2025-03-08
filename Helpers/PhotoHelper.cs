using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimoshStore;
    
    public static class PhotoHelper
    {
        private static readonly string apiKey = "1P_AFRg0FzorbIN0emR0svU81VSDfsAmikPf7gWrco8"; // Buraya Unsplash API anahtarınızı ekleyin

        public static async Task<string> GetRandomPhotoUrlAsync()
        {
            using (var client = new HttpClient())
            {
                // API anahtarınızı Authorization başlığında gönderiyoruz
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {apiKey}");

                // API'den rastgele fotoğraf almak için istek gönderiyoruz
                var response = await client.GetStringAsync("https://api.unsplash.com/photos/random");

                // Eğer istek başarısızsa, hata mesajını göster
                if (string.IsNullOrEmpty(response))
                {
                    throw new Exception("API'den boş yanıt alındı.");
                }

                JArray photoData = null;
                
                // Yanıtın JSON dizisi olup olmadığını kontrol et
                try
                {
                    // JSON'u dizi olarak işlemeye çalışıyoruz
                    photoData = JArray.Parse(response);
                }
                catch (JsonReaderException)
                {
                    // Eğer dizi değilse, nesne olarak işlemeyi deneyelim
                    var jsonObject = JObject.Parse(response);
                    photoData = new JArray(jsonObject); // Nesneyi bir diziye dönüştürüyoruz
                }

                // Fotoğrafın URL'sini alıyoruz
                var photoUrl = photoData[0]["urls"]["regular"].ToString();
                
                return photoUrl;
            }
        }
    }

