using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ImageModel
{
    public string id;
    public string name;
    public string image_url;

    public string Id { 
        get { return id; }
        set { id = value ;}
    }

    public string Name { 
        get { return name; }
        set { name = value; }
    }

    public string Url { 
        get { return image_url; }
        set { image_url = value; }
    }
}
