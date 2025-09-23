using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MeshifAI
{
    [Serializable]
    public class MeshifaiResponse
    {
        public bool success;
        public string download_url;
        public RateLimit rate_limit;

        [Serializable]
        public class RateLimit
        {
            public int hourly_remaining;
            public int burst_remaining;
        }
    }

    public class MeshifaiApiClient
    {
        private const string API_URL = "https://api.meshifai.com/v1/text-to-3d.php";

        /// <summary>
        /// Generates a 3D model from the provided text prompt with the specified quality setting
        /// </summary>
        /// <param name="prompt">Text description of the 3D model to generate</param>
        /// <param name="highRes">Whether to use high resolution mode for better quality (slower generation)</param>
        /// <returns>Response containing download URL and rate limit information</returns>
        public static async Task<MeshifaiResponse> GenerateModelAsync(string prompt, bool highRes)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(prompt))
                throw new ArgumentException("Prompt cannot be empty", nameof(prompt));

            // Create form data
            WWWForm form = new WWWForm();
            form.AddField("prompt", prompt);
            form.AddField("high_res", highRes ? "true" : "false");

            // Create request
            using (UnityWebRequest request = UnityWebRequest.Post(API_URL, form))
            {
                // Set minimal headers
                request.SetRequestHeader("accept", "*/*");
                request.SetRequestHeader("cache-control", "no-cache");

                // Send request
                var operation = request.SendWebRequest();

                // Wait for completion
                while (!operation.isDone)
                    await Task.Delay(100);

                // Check for errors
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"MeshifAI API Error: {request.error}");
                    throw new Exception($"API request failed: {request.error}");
                }

                // Parse response
                string responseText = request.downloadHandler.text;
                try
                {
                    MeshifaiResponse response = JsonUtility.FromJson<MeshifaiResponse>(responseText);
                    return response;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to parse API response: {ex.Message}");
                    Debug.LogError($"Response text: {responseText}");
                    throw new Exception("Failed to parse API response", ex);
                }
            }
        }

        /// <summary>
        /// Downloads a 3D model from the provided URL
        /// </summary>
        /// <param name="url">URL of the 3D model to download</param>
        /// <returns>Downloaded model as a byte array</returns>
        public static async Task<byte[]> DownloadModelAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be empty", nameof(url));

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Delay(100);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Model download error: {request.error}");
                    throw new Exception($"Model download failed: {request.error}");
                }

                return request.downloadHandler.data;
            }
        }
    }
}