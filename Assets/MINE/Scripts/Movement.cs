using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zVec = Input.GetAxisRaw(GetComponent<InputMethod>().horizontal);
        float xVec = Input.GetAxisRaw(GetComponent<InputMethod>().vertical);
        Vector3 moveVec = new Vector3(-xVec, 0, zVec).normalized;
        moveVec.x /= 1.5f;

        this.gameObject.GetComponent<CharacterController>().Move(moveVec * speed * Time.deltaTime);
    }
}
