using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PageModel
{
    public int count;
    public string next;
    public string previous;
    public List<ImageModel> results;

    public int Count { 
        get { return count; }
    }

    public string Next { 
        get { return next; }
    }

    public string Previous { 
        get { return previous; }
    }

    public List<ImageModel> Results { 
        get { return results; }
    }
}
