using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    public ImageAPI _imageApi;
    public ImageLoader _imageLoader;
    private ImageCache _imageCache;

    void Awake() {
        _imageCache = new ImageCache();
    }

    // Start is called before the first frame update
    void Start()
    {
        const int kPageToRequest = 1;
        StartCoroutine(FetchPageOfImageModels(kPageToRequest));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FetchPageOfImageModels(int pageId) {
        Debug.Log("Fetching images");

        List<ImageModel> fetchedResult = null;
        System.Action<List<ImageModel>> callback = (List<ImageModel> result) => {
            fetchedResult = result;
        };
        yield return StartCoroutine(_imageApi.FetchImages(pageId, callback));

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
                Assert.IsTrue(loadedTextureResults.Count == fetchedResult.Count);
                for(int i = 0; i < loadedTextureResults.Count; i++) {
                    if (loadedTextureResults[i] == null) {
                        Debug.Log($"Failed to load texture for url={fetchedResult[i].Url}");
                    } else {
                        Debug.Log($"successfully loaded texture for url={fetchedResult[i].Url}");
                    }
                }
                
                // construct the view models and put them in the cache.
                List<ImageViewModel> viewModels = new List<ImageViewModel>();
                for (int i = 0; i < fetchedResult.Count; i++) {
                    ImageModel model = fetchedResult[i];
                    ImageViewModel viewModel = new ImageViewModel(model);
                    Texture2D texture = loadedTextureResults[i];
                    if (texture != null) {
                        viewModel.Load(texture);
                        _imageCache.AddTexture(model.Id, texture);
                    }
                    viewModels.Add(viewModel);
                }
                _imageCache.AddViewModels(viewModels);
            }
        }
    }
}
