using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// API Requirements: a mocked API endpoint delivering paginated data, including an 'id', 'name', and 'image url' for each item.
public interface IImageAPI {

    // Fetch limit# of image models 
    IEnumerator FetchImages(int page, System.Action<List<ImageModel>> callback);
}