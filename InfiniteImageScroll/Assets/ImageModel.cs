using System.Collections;
using System.Collections.Generic;

public class ImageModel : IImageModel
{
    private string _id;
    public string Id { 
        get { return _id; }
        set { _id = value ;}
    }

    private string _name;
    public string Name { 
        get { return _name; }
        set { _name = value; }
    }

    private string _url;
    public string Url { 
        get { return _url; }
        set { _name = value; }
    }
}
