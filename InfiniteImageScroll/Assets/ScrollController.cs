using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;

public class ScrollController : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    // data list
    private Dictionary<string, ImageViewModel> _viewModels;

    // Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        _viewModels = new Dictionary<string, ImageViewModel>();
       // _recyclableScrollRect.DataSource = this;
    }

    // Initializing view models
    public void InitViewModels(Dictionary<string, ImageViewModel> viewModels) 
    {
        _viewModels = viewModels;
        _recyclableScrollRect.Initialize(this);
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _viewModels.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        // Casting to the implemented Cell
        var item = cell as ImageCell;
        string modelId = index.ToString();
        if (!_viewModels.ContainsKey(modelId)) {
            Debug.LogError($"Calling Setcell on a viewModelId={modelId}, which does not exist yet.");
            return;
        }
        ImageViewModel viewModel = _viewModels[modelId];
        item.ConfigureCell(viewModel, modelId);
    }

    #endregion
}

