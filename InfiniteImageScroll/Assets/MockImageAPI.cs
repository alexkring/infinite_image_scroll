using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Loads a mock set of images from a json file, for testing the image api.
public class MockImageAPI : IImageAPI
{
    private List<ImageModel> _imageModelList;

    public MockImageAPI(string jsonFilepath) {
        _imageModelList = new List<ImageModel>();
    }
    
    // Fetch limit# of image models 
    public async Task<List<IImageModel>> FetchImages(int page) {
        return null;
        /*
        if (offset + limit > _imageModelList.Count) {
            Debug.LogError($"could not fetch images because you are trying to fetch beyond the bounds of the images in the mock repo");
            return null;
        }
        List<IImageModel> imageModelsFound = new List<IImageModel>(limit);
        for (int i = offset; i < limit; i++) {
            ImageModel model = _imageModelList[i];
            imageModelsFound.Add(model);
        }
        return imageModelsFound;
        */
    }
}