using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface ITextureRequestHandler {
    IEnumerator LoadTexture(string modelId, string url, System.Action<Texture2D> callback);
}

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour, ITextureRequestHandler
{
    public ImageAPI _imageApi;
    public TextureLoader _textureLoader;
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
            scrollableList.Sort( delegate (ImageViewModel x, ImageViewModel y) { 
                    int xId = 0;
                    int yId = 0;
                    Int32.TryParse(x.Id, out xId);
                    Int32.TryParse(y.Id, out yId);
                    return (xId.CompareTo(yId)); 
                } 
            );
            _scrollController.InitViewModels(scrollableList, this);
        }));
    }

    public IEnumerator LoadTexture(string modelId, string url, System.Action<Texture2D> callback) {
        Texture2D result = null;
        // TODO: check if item exists in cache and return it first, if request is unnecessary.
        yield return StartCoroutine(_textureLoader.LoadTexture(url, (Texture2D r) => {
            result = r;
        }));
        if (result != null) {
            _imageCache.AddTexture(modelId, result);
        }
        callback(result);
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
        }
    }
}
