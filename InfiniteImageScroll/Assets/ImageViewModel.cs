using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageViewModel
{
    public string Id {
        get { return _model.Id; }
    }
    public string Name {
        get { return _model.Name; }
    }

    public string Url {
        get { return _model.Url; }
    }

    public bool IsLoaded {
        get { return (_texture != null); }
    }

    private Texture2D _texture;
    public Texture2D Texture {
        get { return _texture; }
    }

    private ImageModel _model;

    public ImageViewModel(ImageModel model) {
        _model = model;
        _texture = null;
    }

    public void Load(Texture2D texture) {
        _texture = texture;
    }

    public void Unload() {
        _texture = null;
    }
}
