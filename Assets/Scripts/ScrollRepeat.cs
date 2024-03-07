using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRepeat : MonoBehaviour
{
    private float height;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        height = boxCollider.size.y; // Get the width of the background image and multiply it by the scale of the background image to get the actual width
    }
    
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -height)
        {
            Reposition();
        }
    }

    // Reposition the background to the bottom part
    private void Reposition()
    {
       transform.position = new Vector3(0, height, 0); // Set the position of the background to the top part of the screen
    }
}
