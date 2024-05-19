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
        const int kStartPage = 1;
        const int kNumPages = 4;
        StartCoroutine(FetchMultiplePageOfImageModels(kStartPage, kNumPages));
    }

    IEnumerator FetchMultiplePageOfImageModels(int startPage, int numPages) {
        int end = startPage + numPages;
        for (int page = startPage; page < end; page++) {
             yield return StartCoroutine(FetchPageOfImageModels(page));
        }
    }

    IEnumerator FetchPageOfImageModels(int pageId) {
        Debug.Log("Fetching images");

        List<ImageModel> fetchedResult = null;
        yield return StartCoroutine(_imageApi.FetchImages(pageId, (List<ImageModel> result) => {
            fetchedResult = result;
        }));

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
                
                // construct the view models and put them and the textures in the cache.
                for (int i = 0; i < fetchedResult.Count; i++) {
                    ImageModel model = fetchedResult[i];
                    ImageViewModel viewModel = new ImageViewModel(model);
                    Texture2D texture = loadedTextureResults[i];
                    if (texture != null) {
                        viewModel.Load(texture);
                        _imageCache.AddTexture(model.Id, texture);
                    }
                    _imageCache.AddViewModel(model.Id, viewModel);
                }
            }
        }
    }
}
