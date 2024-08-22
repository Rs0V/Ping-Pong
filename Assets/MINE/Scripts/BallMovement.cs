using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    Vector3 hitPos;
    Vector3 nextPos;
    public Vector2 curvePoint; // x = pos on line(0f - 1f); y = height(> 0f);
    Vector3 curveVert;
    float hitForce;
    float lerp;
    bool hit;

    Vector3 lastPos;
    bool bounce;
    Vector3 bounceDir;
    public float minY;
    public float gravity;
    float g;

    float bounceGravMulti;

    public GameObject tableCollider;

    public GameObject sceneMngr;
    public SphereCollider inGameColl;
    public SphereCollider loseGameColl;
    public Rigidbody loseGameRB;

    // Start is called before the first frame update
    void Start()
    {
        inGameColl.enabled = true;
        loseGameColl.enabled = false;
        loseGameRB.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(hit == true)
        {
            if (lerp < 0.995f)
            {
                lastPos = transform.position;
                lerp += hitForce * Time.deltaTime;
                Vector3 temp = Vector3.Lerp(hitPos, curveVert, lerp);
                transform.position = Vector3.Lerp(temp, nextPos, lerp);
            }
            else
            {
                transform.position = nextPos;
                hit = false;
                bounce = true;
                bounceDir = (transform.position - lastPos).normalized;
                bounceDir = Quaternion.AngleAxis(-Vector3.Angle(new Vector3(-1f, 0f, 0f), bounceDir) * 2f, new Vector3(0f, 0f, 1f)) * bounceDir;
            }
        }
        else if(bounce == true)
        {
            g += gravity * Time.deltaTime;
            transform.position += (bounceDir * hitForce * 1.5f + new Vector3(bounceDir.x, 0f, bounceDir.z).normalized * hitForce * 1.2f
                                  + new Vector3(0f, -g, 0f) * bounceGravMulti) * Time.deltaTime;
            if(transform.position.y < minY)
            {
                Vector3 temp = transform.position;
                temp.y = minY;
                transform.position = temp;
                bounce = false;
                g = 0f;

                sceneMngr.GetComponent<SceneManagement>().RestartScene(this.gameObject,
                                                                      (bounceDir * hitForce * 1.5f
                                                                      + new Vector3(bounceDir.x, 0f, bounceDir.z).normalized
                                                                      * hitForce * 1.2f));
                Destroy(tableCollider.GetComponent<Rigidbody>());
            }
        }
    }

    public void NextPos(Vector3 pos, float force, float gravMultiplier)
    {
        hitPos = transform.position;
        nextPos = pos;
        lerp = 0f;
        curveVert = Vector3.Lerp(transform.position, nextPos, curvePoint.x);
        curveVert.y = transform.position.y + curvePoint.y;
        hitForce = force / (Vector3.Distance(transform.position, nextPos));
        hit = true;
        bounce = false;
        g = 0f;
        bounceGravMulti = 2 - gravMultiplier;
    }

    public void DoubleBounce()
    {
        if (bounce == true && g > gravity * Time.deltaTime * 3f)
        {
            Vector3 temp = transform.position;
            temp.y = nextPos.y;
            transform.position = temp;
            bounce = false;
            g = 0f;

            sceneMngr.GetComponent<SceneManagement>().RestartScene(this.gameObject,
                                                                  (bounceDir * hitForce * 1.5f
                                                                  + new Vector3(bounceDir.x, 0f, bounceDir.z).normalized
                                                                  * hitForce * 1.2f));
            Destroy(tableCollider.GetComponent<Rigidbody>());
        }
    }
}
