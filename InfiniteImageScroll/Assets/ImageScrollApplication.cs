using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    private ImageLoader _imageLoader;
    private ImageAPI _imageApi;

    void Awake() {
        _imageLoader = new ImageLoader();
        _imageApi = new ImageAPI();
    }

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

        Task<List<ImageModel>> task = Task.Run(() =>_imageApi.FetchImages(1));
        yield return new WaitUntil(() => task.IsCompleted);
        
        List<ImageModel> imageModels = task.Result;
        if ( imageModels == null ) {
             Debug.Log($"Failed to retrieve any image models");
        } else {
             Debug.Log($"Successfully retrieved {imageModels.Count} image models");
        }
    }
}
