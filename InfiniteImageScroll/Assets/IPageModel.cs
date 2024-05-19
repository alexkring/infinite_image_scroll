using System.Collections;
using System.Collections.Generic;

public interface IPageModel
{
    int Count { get; }
    string Next { get; }
    string Previous { get; }
    //List<ImageModel> Results { get; }
}
