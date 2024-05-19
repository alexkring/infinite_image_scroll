using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IImageModel {
    string Id { get; }
    string Name { get; }
    string Url { get; }
}

// API Requirements: a mocked API endpoint delivering paginated data, including an 'id', 'name', and 'image url' for each item.
public interface IImageAPI {

    // Fetch limit# of image models 
    Task<List<ImageModel>> FetchImages(int page);
}