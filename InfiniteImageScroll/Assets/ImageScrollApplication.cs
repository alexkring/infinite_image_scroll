using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    public ImageAPI _imageApi;
    public ImageLoader _imageLoader;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchImageModels());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FetchImageModels() {
        Debug.Log("Fetching images");

        List<ImageModel> fetchedResult = null;
        System.Action<List<ImageModel>> callback = (List<ImageModel> result) => {
            fetchedResult = result;
        };
        yield return StartCoroutine(_imageApi.FetchImages(1, callback));

        if ( fetchedResult == null ) {
             Debug.Log($"Failed to retrieve any image models");
        } else {
             Debug.Log($"Successfully retrieved {fetchedResult.Count} image models");
             foreach (ImageModel model in fetchedResult) {
                Debug.Log($"model: {model.Id}, {model.Name}, {model.Url}");
             }

            List<Texture2D> loadedTextureResults = null;
            yield return StartCoroutine(_imageLoader.LoadImages(fetchedResult, (List<Texture2D> result) => {
                loadedTextureResults = result;
            }));

            if (loadedTextureResults == null) {
                Debug.LogError("Failed to load textures");
            } else {
                Debug.Log($"Successfully retrieved {loadedTextureResults.Count} textures");
            }
        }
    }
}
