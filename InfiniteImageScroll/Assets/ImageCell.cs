using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

// Cell class that get events from the recyclable UI controller. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
// The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
// The configuration of a cell is done through the DataSource SetCellData method.
public class ImageCell : MonoBehaviour, ICell
{
    // UI
    public Text idLabel;
    public Text nameLabel;
    public Text urlLabel;

    // Model
    private ImageViewModel _viewModel;
    private string _modelId;

    private void Start()
    {
        // Can also be done in the inspector
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    // This is called from the SetCell method in DataSource
    public void ConfigureCell(ImageViewModel model, string modelId)
    {
        _modelId = modelId;
        _viewModel = model;

        idLabel.text = model.Id;
        nameLabel.text = model.Name;
        urlLabel.text = model.Url;

        Debug.Log($"Configuring Cell=> modelId : {modelId}, Name : {model.Name}, Url {model.Url}");
    }

    
    private void ButtonListener()
    {
        Debug.Log("modelId : " + _modelId +  ", Name : " + _viewModel.Name  + ", Url : " + _viewModel.Url);
    }
}
