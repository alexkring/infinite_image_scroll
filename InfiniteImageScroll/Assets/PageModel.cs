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
