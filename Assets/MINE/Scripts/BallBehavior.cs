using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBehavior : MonoBehaviour
{
    public GameObject ballTemplate;
    public GameObject tableTrigger;

    [HideInInspector]
    public GameObject ball;

    public GameObject landPlat;
    //float xLandSize = 0f;
    //float zLandSize = 0f;

    bool touched = false;
    public float hitForce = 10f;
    public float xFacing = -1f;

    [HideInInspector]
    public bool serve;

    public GameObject friend;

    public float minXDist; // > 0
    public float maxXDist; // > 0
    public float minZDist; // < 0
    public float maxZDist; // > 0

    public Canvas hitBlink;
    public float hitBlinkAlpha = 1f;
    bool fadeHBOut;
    float fadeHB;
    public float fadeHBMultiplier = 1f;

    bool FirstHit;
    public GameObject sceneMngr;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(touched == true)
        {
            if(Input.GetAxisRaw(GetComponent<InputMethod>().hit) > 0f)
            {
                float lP_y = landPlat.transform.position.y;
                float ball_y = ball.transform.position.y;
                int cond = 1;
                if (ball_y > lP_y)
                {
                    cond = 0;
                }
                float distFromGrdChange = cond * ((1 - (lP_y - ball_y) / lP_y) + 0.5f) / 1.5f + 1 - cond;

                ball.GetComponent<BallMovement>().NextPos(landPlat.transform.position
                                                          + new Vector3(xFacing * Random.Range(minXDist,
                                                                       (maxXDist - minXDist) * distFromGrdChange + minXDist),
                                                                       0f, Random.Range(minZDist, maxZDist)), hitForce, distFromGrdChange);
                if(FirstHit == false)
                {
                    sceneMngr.GetComponent<SceneManagement>().FirstHit();
                }
            }
        }
        if(fadeHBOut == true)
        {
            fadeHB += Time.deltaTime * fadeHBMultiplier;
            Color temp = hitBlink.GetComponent<Image>().color;
            temp.a -= fadeHB;
            if(temp.a < 0f)
            {
                temp.a = 0f;
                fadeHBOut = false;
            }
            hitBlink.GetComponent<Image>().color = temp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball)
        {
            touched = true;
            other.GetComponent<MeshRenderer>().material.color = Color.red;

            Color temp = hitBlink.GetComponent<Image>().color;
            temp.a = hitBlinkAlpha;
            hitBlink.GetComponent<Image>().color = temp;
            fadeHB = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ball)
        {
            touched = false;
            other.GetComponent<MeshRenderer>().material.color = Color.white;

            fadeHBOut = true;
        }
    }

    public void Initialize()
    {
        if (serve == true)
        {
            ball = Instantiate<GameObject>(ballTemplate);
            ball.transform.position = transform.position + transform.forward * 0.3f;
            //xLandSize = landPlat.GetComponent<MeshFilter>().mesh.bounds.extents.x / 2f;
            //zLandSize = landPlat.GetComponent<MeshFilter>().mesh.bounds.extents.z / 2f;

            friend.GetComponent<BallBehavior>().ball = ball;
            tableTrigger.GetComponent<DoubleBounce>().ball = ball;
            Destroy(ballTemplate);
        }
    }
}
