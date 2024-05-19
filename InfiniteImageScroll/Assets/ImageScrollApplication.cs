using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to initialize the application with the correct state, and to handle any unity event lifecycle management to communicate with other code components.
public class ImageScrollApplication : MonoBehaviour
{
    private ImageLoader _imageLoader;

    void Awake() {
        _imageLoader = new ImageLoader();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
