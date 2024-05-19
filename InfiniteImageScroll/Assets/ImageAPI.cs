using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Loads a mock set of images from https://hyper-image-test.netlify.app/.netlify/functions/images
public class ImageAPI : IImageAPI
{
    private List<ImageModel> _imageModelList;

    private const string kBaseUrl = "https://hyper-image-test.netlify.app/.netlify/functions/images";

    public ImageAPI() {
        _imageModelList = new List<ImageModel>();
    }
    
    // Fetch limit# of image models 
    public async Task<List<IImageModel>> FetchImages(int page) {

        List<IImageModel> imageModelsFound = new List<IImageModel>();
        ImageModel model = new ImageModel();
        model.Name = "";
        model.Id = "";
        model.Url = "";
        imageModelsFound.Add(model);
        return imageModelsFound;
    }

    //private async Task<

    /*
    private async Task<ImageModel> LoadImage(string url) 
    {
        using( UnityWebRequest www = UnityWebRequest.Get(url) )
        {
            var asyncOp = www.SendWebRequest();
            while( !asyncOp.isDone ) {
                const int DelayInMilliseconds = 30;
                await Task.Delay( DelayInMilliseconds );
            }
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
                return null;
            }
            else {
                Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                return myTexture;
            }
        }
    }
    */
}
