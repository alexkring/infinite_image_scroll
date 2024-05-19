using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PageModel : IPageModel
{
    private int count;
    private string next;
    private string previous;
    private List<ImageModel> results;

    public int Count { 
        get { return count; }
        set { count = value ;}
    }

    public string Next { 
        get { return next; }
        set { next = value; }
    }

    public string Previous { 
        get { return previous; }
        set { previous = value; }
    }

    public List<ImageModel> Results { 
        get { return results; }
        set { results = value; }
    }
}
