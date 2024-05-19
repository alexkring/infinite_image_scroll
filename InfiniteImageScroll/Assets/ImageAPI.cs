using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Loads a mock set of images from https://hyper-image-test.netlify.app/.netlify/functions/images
public class ImageAPI : MonoBehaviour, IImageAPI
{
    private const string kBaseUrl = "https://hyper-image-test.netlify.app/.netlify/functions/images";

    // Fetch a page of images 
    public IEnumerator FetchImages(int page, System.Action<List<ImageModel>> callback) 
    {
        string url = $"{kBaseUrl}?page={page}";
        Debug.Log($"Requesting url {url}");
        using( UnityWebRequest www = UnityWebRequest.Get(url) )
        {
            yield return www.SendWebRequest();
            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + www.error);
                    callback(null);
                    yield break;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + www.error);
                    callback(null);
                    yield break;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received web request successfully");
                    break;
            }

            PageModel pageModel = null;
            string text = www.downloadHandler.text;
            Debug.Log($"Retrieve page {page} => {text}");
            try {
                pageModel = JsonUtility.FromJson<PageModel>(text);
            } catch (Exception e) {
                Debug.LogError(e.Message);
            }

            if (pageModel == null) {
               callback(null);
            } else  {
                callback(pageModel.Results);
            }
        }
    }
}
