using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// Loads a mock set of images from https://hyper-image-test.netlify.app/.netlify/functions/images
public class ImageAPI : IImageAPI
{
    private List<ImageModel> _imageModelList;

    private const string kBaseUrl = "https://hyper-image-test.netlify.app/.netlify/functions/images";

    public ImageAPI() {
        _imageModelList = new List<ImageModel>();
    }
    
    // Fetch a page of images 
    public async Task<List<ImageModel>> FetchImages(int page) 
    {
        string url = $"{kBaseUrl}?page={page}";
        Debug.Log($"Requesting url {url}");
        using( UnityWebRequest www = UnityWebRequest.Get(url) )
        {
            var asyncOp = www.SendWebRequest();
            while( !asyncOp.isDone ) 
            {
                const int DelayInMilliseconds = 30;
                await Task.Delay( DelayInMilliseconds );
            }
            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
                return null;
            }
            else 
            {
                PageModel pageModel = null;
                string text = www.downloadHandler.text;
                Debug.Log($"Retrieve page {page} => {text}");
                try {
                    pageModel = JsonUtility.FromJson<PageModel>(text);
                } catch (Exception e) {
                    Debug.LogError(e.Message);
                }
                if (pageModel == null) {
                    return null;
                }
                return pageModel.Results;
            }
        }

        /*
        List<IImageModel> imageModelsFound = new List<IImageModel>();
        ImageModel model = new ImageModel();
        model.Name = "";
        model.Id = "";
        model.Url = "";
        imageModelsFound.Add(model);
        return imageModelsFound;
        */
    }
}
