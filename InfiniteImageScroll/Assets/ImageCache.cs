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


    private Dictionary<string, ImageViewModel> _viewModels; // modelId to ImageViewModel mapping
    private Dictionary<string, Texture2D> _textures; // modelId to texture mapping

    public ImageCache(int viewModelSize = 5, int textureSize = 5) {
        _viewModelSize = viewModelSize;
        _viewModels = new Dictionary<string, ImageViewModel>();
        _textures = new Dictionary<string, Texture2D>();
    }

    public void AddViewModel(string modelId, ImageViewModel viewModel) {
        if (_viewModels.ContainsKey(modelId)) {
            Debug.LogError($"Trying to add modelId={modelId} ViewModel to cache, but it already exists.");
            return;
        }
        // TODO: eviction logic
        _viewModels[modelId] = viewModel;
        Debug.Log($"Added Id={modelId} ViewModel to cache");
    }

    public void AddTexture(string modelId, Texture2D texture) {
        if (_textures.ContainsKey(modelId)) {
            Debug.LogError($"Trying to add modelId={modelId} texture to cache, but it already exists.");
            return;
        }
        // TODO: eviction logic
        _textures[modelId] = texture;
        Debug.Log($"Added Id={modelId} texture to cache");
    }
}
