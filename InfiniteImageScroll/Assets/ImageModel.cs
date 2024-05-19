using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ImageModel : IImageModel
{
    private string id;
    public string Id { 
        get { return id; }
        set { id = value ;}
    }

    private string name;
    public string Name { 
        get { return name; }
        set { name = value; }
    }

    private string image_url;
    public string Url { 
        get { return image_url; }
        set { image_url = value; }
    }
}
