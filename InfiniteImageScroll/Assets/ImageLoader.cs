using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

// Loads remote web images into 2d textures in memory.
public class ImageLoader
{
    public async Task<List<Texture2D>> LoadNextPage(List<string> urlList)
    {
        Debug.Log("Loading next page of textures");

        List<Texture2D> texturesLoaded = new List<Texture2D>();
        foreach (string url in urlList) {
            Texture2D texture = await LoadTexture(url);
            texturesLoaded.Add(texture);
        }
        return texturesLoaded;
    }

    private async Task<Texture2D> LoadTexture(string url) 
    {
        using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
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
}
