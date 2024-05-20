using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

// Loads remote web images into 2d textures in memory.
public class TextureLoader : MonoBehaviour
{
    public IEnumerator LoadTextures(List<ImageModel> modelList, System.Action<List<Texture2D>> callback)
    {
        List<Texture2D> texturesLoaded = new List<Texture2D>();
        foreach (ImageModel model in modelList) {
            yield return StartCoroutine(LoadTexture(model.Url, (Texture2D result) => {
                texturesLoaded.Add(result);
            }));
        }
        callback(texturesLoaded);
    }

    public IEnumerator LoadTexture(string url, System.Action<Texture2D> callback) 
    {
        using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
        {
            yield return www.SendWebRequest();
            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTP Error: url={url}, error={www.error}");
                    callback(null);
                    yield break;
                case UnityWebRequest.Result.Success:
                    Debug.Log($"Received web request successfullyc for url={url}");
                    break;
            }

            Texture2D result = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callback(result);
        }
    }
}
