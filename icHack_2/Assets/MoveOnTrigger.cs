using UnityEngine;
using System.Collections;

public class MoveOnTrigger : MonoBehaviour {

    Vector3 lookDir;
    Quaternion crtrot;
    public float speed = 1.0f;
    public bool trig = false;
    Vector3 loc, locfy;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
	    
	}
	
   

	// Update is called once per frame
    void EarlyUpdate()
    {

    }
	void Update () {
        crtrot = Cardboard.SDK.HeadPose.Orientation;
        lookDir = crtrot * Vector3.forward;
        if (Cardboard.SDK.Triggered)
        {
            trig = !trig;
        }
        if (trig)
        {
            transform.position += lookDir * speed;
        }
    }
    void LateUpdate()
    {
        loc = transform.position;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        locfy = new Vector3(loc.x, hit.point.y+2.0f, loc.z);
        transform.position = locfy;
    }
}
