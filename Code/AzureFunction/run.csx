using System;
using System.Net;

public static void Run(string message, TraceWriter log)
{
    float temp = 0;
    if (float.TryParse(message, out temp))
    {
        log.Info($"C# Queue trigger function processed: {message}");

        string color = "green";
        if (temp > 76)
            color = "red";

        string particleAccessToken = "<ParticleAccessToken>";
        string particleDeviceId = "<ParticleDeviceId>";
        string particleFunction = "lightLed";        
        string particleWebHookUri = $"https://api.particle.io/v1/devices/{particleDeviceId}/{particleFunction}?access_token={particleAccessToken}";
        string particleWebHookArgs = $"args={color}";

        using (WebClient wc = new WebClient())
        {
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadString(particleWebHookUri, particleWebHookArgs);
        }
    }
}