using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    public ImageAPI _imageApi;
    public ImageLoader _imageLoader;
    public ScrollController _scrollController;
    private ImageCache _imageCache;

    void Awake() {
        _imageCache = new ImageCache();
    }

    void Start()
    {
        // Note: we could be more intelligent about requesting these fetches when the user is toward the end of a scroll.
        // For now, we are just going to request all 5 pages of images since we know there are 5.
        const int kStartPage = 1;
        const int kNumPages = 5;
        StartCoroutine(FetchMultiplePageOfImageModels(kStartPage, kNumPages, (bool result) => {
            // Set data source on the ui controller
            List<ImageViewModel> scrollableList = new List<ImageViewModel>();
            foreach (ImageViewModel model in _imageCache.ViewModels.Values) {
                scrollableList.Add(model);
            }
            _scrollController.InitViewModels(scrollableList);
        }));
    }

    IEnumerator FetchMultiplePageOfImageModels(int startPage, int numPages, System.Action<bool> callback) {
        int end = startPage + numPages;
        for (int page = startPage; page < end; page++) {
             yield return StartCoroutine(FetchPageOfImageModels(page));
             // Note: we could call _scrollController.InitViewModels(_imageCache.ViewModels) here, if we want the UI models to update with each page. But it looks cleaner
            // If we do it after a batch of requests.
        }
        callback(true);
    }

    IEnumerator FetchPageOfImageModels(int pageId) {
        Debug.Log($"Fetching page {pageId} ImageModels");

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

             // Construct the view models and put them in the cache.
            for (int i = 0; i < fetchedResult.Count; i++) {
                ImageModel model = fetchedResult[i];
                ImageViewModel viewModel = new ImageViewModel(model);
                _imageCache.AddViewModel(model.Id, viewModel);
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
                
                // Construct the view models and put them and the textures in the cache.
                for (int i = 0; i < fetchedResult.Count; i++) {
                    ImageModel model = fetchedResult[i];
                    ImageViewModel viewModel = _imageCache.GetViewModel(model.Id);
                    Texture2D texture = loadedTextureResults[i];
                    if (texture != null) {
                        viewModel.Load(texture);
                        _imageCache.AddTexture(model.Id, texture);
                    }
                }
            }
        }
    }
}
