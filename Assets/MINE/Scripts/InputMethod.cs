using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMethod : MonoBehaviour
{
    public int player_number;

    [HideInInspector]
    public string horizontal;

    [HideInInspector]
    public string vertical;

    [HideInInspector]
    public string hit;

    // Start is called before the first frame update
    void Start()
    {
        if(player_number == 1)
        {
            horizontal = "Horizontal(left)";
            vertical = "Vertical(left)";
            hit = "Hit(left)";
        }
        else
        {
            horizontal = "Horizontal(right)";
            vertical = "Vertical(right)";
            hit = "Hit(right)";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
