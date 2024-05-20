using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;

public class ScrollController : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    // data list
    private List<ImageViewModel> _viewModels;
    private ITextureRequestHandler _textureRequestHandler;

    // Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _viewModels = new List<ImageViewModel>();
    }

    // Initializing view models
    public void InitViewModels(List<ImageViewModel> viewModels, ITextureRequestHandler textureRequestHandler) 
    {
        _viewModels = viewModels;
        _textureRequestHandler = textureRequestHandler;
        Debug.Log($"InitViewModels called with {viewModels.Count} models");
        _recyclableScrollRect.Initialize(this);
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        Debug.Log($"GetItemCount called with {_viewModels.Count} models");
        return _viewModels.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        if (index >= _viewModels.Count) {
            Debug.LogError($"Calling Setcell on a indexId={index}, which does not exist yet.");
            return;
        }

        // Casting to the implemented Cell
        var item = cell as ImageCell;
        ImageViewModel viewModel = _viewModels[index];
        item.ConfigureCell(viewModel, viewModel.Id, _textureRequestHandler);
    }

    #endregion
}

