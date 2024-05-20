using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

// Cell class that get events from the recyclable UI controller. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
// The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
// The configuration of a cell is done through the DataSource SetCellData method.
// Honestly this class is very similar to the viewmodel, but it also has a monobehahvior. I would probably want to refactor and combine this with the 
// ImageViewModel class since they are the same thing. But I wrote this code separately when testing the PolyAndCode.UI namespace ^_^
public class ImageCell : MonoBehaviour, ICell
{
    // UI
    public Text idLabel;
    public Text nameLabel;
    public Text urlLabel;
    public Image image;
    public GameObject LoadingState;

    // Model
    private ImageViewModel _viewModel;
    private string _modelId;
    private ITextureRequestHandler _textureRequestHandler;
    private bool _isLoading = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    public void ClearCell() {
        Debug.Log($"Clearing Cell=> modelId : {_modelId}");
        _modelId = "";
        _viewModel = null;
        idLabel.text = "Id";
        nameLabel.text = "Name";
        urlLabel.text = "Url";
    }

    // This is called from the SetCell method in DataSource
    public void ConfigureCell(ImageViewModel model, string modelId,  ITextureRequestHandler textureRequestHandler)
    {
        _modelId = modelId;
        _viewModel = model;
        _textureRequestHandler = textureRequestHandler;

        idLabel.text = model.Id;
        nameLabel.text = model.Name;
        urlLabel.text = model.Url;

        Debug.Log($"Configuring Cell=> modelId : {modelId}, Name : {model.Name}, Url {model.Url}");
    }

    private void ButtonListener()
    {
        Debug.Log("modelId : " + _modelId +  ", Name : " + _viewModel.Name  + ", Url : " + _viewModel.Url);
        
        // load / unload texture
        if (!_viewModel.IsLoaded) {
            if (!_isLoading) {
                _isLoading = true;
                LoadingState.SetActive(true);
                StartCoroutine(LoadTexture());
            }
        } else {
            // TODO: unload texture
        }
    }

    private IEnumerator LoadTexture() {
        yield return StartCoroutine(_textureRequestHandler.LoadTexture(_viewModel.Id, _viewModel.Url, (Texture2D texture) => {
            _isLoading = false;
            LoadingState.SetActive(false);
            if (texture != null) {
                Debug.Log($"Successfully loaded texture for modelId={_viewModel.Id}, url={_viewModel.Url}");
                _viewModel.Load(texture);
                image.material.SetTexture("_MainTex", texture);
                image.gameObject.SetActive(true);
            }
        }));
    }
}
