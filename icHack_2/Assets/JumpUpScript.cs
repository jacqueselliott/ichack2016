using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class JumpUpScript : MonoBehaviour {

    GameObject Main;
    MoveOnTrigger motrig;

    public float speed = 0.1f;
    Vector3 lookDir;
    Quaternion crtrot;
    bool gazingAt = false;
    Vector3 between;

    float yThresh = 1.1f;
    bool crossedFirst = false;
    bool crossedSecond = false;
    float nodStart = 0.0f;
    float nodEnd = 0.0f;
    ParticleSystem glow;
    int ii = 0;
    Quaternion initrot;

    public void SetGazedAt(bool gazedAt)
    {
        //GetComponent<Renderer>().material.color = gazedAt ? Color.black : Color.white;

        glow = GetComponent<ParticleSystem>();
        if (gazedAt == true)
        {
            yThresh = lookDir.y;
            gazingAt = true;
            //print("seen");
            glow.Play();
        }
        else if (gazedAt == false)
        {
            gazingAt = false;
            //print("away");
            glow.Clear();
            glow.Stop();
        }
    }

    private void SelectActivate(bool selected)
    {
        if (selected)
        {
            //GetComponent<Renderer>().material.color = Color.blue;
            var lookPos = Main.transform.position - transform.position;
           // Quaternion initrotz=Quaternion.identity;
            //initrotz.eulerAngles = new Vector3(0, 0, initrot.eulerAngles.z);
            var rotation = Quaternion.LookRotation(lookPos);
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, initrot.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Main.transform.position - transform.position), Time.deltaTime*speed);


            //transform.eulerAngles += new Vector3(0, 0, initrot.eulerAngles.z);
        }
        else
        {
            //GetComponent<Renderer>().material.color = Color.white;
           //transform.rotation = initrot;
           transform.rotation = Quaternion.Slerp(transform.rotation, initrot, Time.deltaTime * speed);
        }
    }

    // Use this for initialization
    void Start()
    {
        //string path = @"C:\pic1";
        SetGazedAt(false);
        initrot = transform.rotation;
        Main = GameObject.Find("CardboardMain");
        motrig = Main.GetComponent<MoveOnTrigger>();
        //var rend = GetComponent<Renderer>();
        //rend.material.mainTexture = Resources.Load("pic1") as Texture;
        //rend.material.mainTexture = LoadPicture(path) as Texture;
    }

    // Update is called once per frame
    void Update()
    {
       
        crtrot = Cardboard.SDK.HeadPose.Orientation;
        lookDir = crtrot * Vector3.forward;
        // print(lookDir);

        between = this.transform.position - Main.transform.position;

        if (gazingAt)
        {
            if (between.magnitude < 6)
            {
                SelectActivate(true);

                //this.gameObject.transform.position += Vector3.up * Time.deltaTime * speed;
                //Main.transform.position += between * speed*Time.deltaTime;
                if (between.magnitude < 4.5)
                {
                    if (motrig.trig)
                    {
                        motrig.trig = false;
                    }
                }
                
            }
        }
        else if (!gazingAt)
        {
            SelectActivate(false);
            //this.gameObject.transform.position -= Vector3.up * Time.deltaTime * speed;
        }

        /*if(crossedFirst && lookDir.y > yThresh-0.1)
        {
            print("done ");
            crossedFirst = false;
            nodEnd = Time.time;
            if ((nodEnd - nodStart) < 1.5 && crossedSecond)
            {
                nodActivate();
                crossedSecond = false;
            }
            crossedSecond = false;
        }
        if (!crossedSecond && lookDir.y < yThresh-0.2)
        {
            print("Second and First ");
            crossedSecond = true;
            crossedFirst = true;
            if (nodStart == 0.0f)
            {
                nodStart = Time.time;
            }
        }
        else if (!crossedFirst && lookDir.y < yThresh-0.1)
        {
            print("Crossed First");
            crossedFirst = true;
            nodStart = Time.time;
        }*/

       
    }

    public static Texture2D LoadPicture(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;
        //print("trying");
        if (File.Exists(filePath))
        {
            //print("exists");
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2,2);
            tex.LoadImage(fileData);
        }

        return tex;
    }
}
