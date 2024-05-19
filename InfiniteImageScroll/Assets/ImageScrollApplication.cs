using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    public ImageAPI _imageApi;
    private ImageLoader _imageLoader;

    void Awake() {
        _imageLoader = new ImageLoader();
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

        List<ImageModel> fetchedResult = null;
        System.Action<List<ImageModel>> callback = (List<ImageModel> result) => {
            fetchedResult = result;
        };
        yield return StartCoroutine(_imageApi.FetchImages(1, callback));

        if ( fetchedResult == null ) {
             Debug.Log($"Failed to retrieve any image models");
        } else {
             Debug.Log($"Successfully retrieved {fetchedResult.Count} image models");
        }
    }
}
