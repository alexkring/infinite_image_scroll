using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Storage location for the image results we receive, so that we can keep it
// in memory.
public class ImageCache
{
    // how many items we can store in the cache
    private int _viewModelSize;
    private int _textureSize;


    private List<ImageViewModel> _viewModels;
    private Dictionary<string, Texture2D> _textures; // modelId to texture mapping

    public ImageCache(int viewModelSize = 5, int textureSize = 5) {
        _viewModelSize = viewModelSize;
        _viewModels = new List<ImageViewModel>();
        _textures = new Dictionary<string, Texture2D>();
    }

    public void AddViewModels(List<ImageViewModel> viewModels) {
        if (viewModels.Count > _viewModelSize) {
            Debug.LogError("Could not add view models because too many ar being added");
            return;
        }
        // TODO: eviction logic
        _viewModels.AddRange(viewModels);
    }

    public void AddTexture(string modelId, Texture2D texture) {
         // TODO: eviction logic
        _textures[modelId] = texture;
    }
}
