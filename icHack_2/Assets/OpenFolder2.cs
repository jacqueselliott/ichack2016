using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;



public class OpenFolder2 : MonoBehaviour
{

    GameObject Main;
    MoveOnTrigger motrig;
    GameObject Glob;

    public float speed = 0.1f;
    Vector3 lookDir;
    Quaternion crtrot;
    bool gazingAt = false;
    Vector3 between;

    float t;
    bool crossedFirst = false;
    bool crossedSecond = false;
    float nodStart = 0.0f;
    float nodEnd = 0.0f;
    ParticleSystem glow;
    int ii = 0;
    Quaternion initrot;
    bool opening = false, open = false;
    private GameObject lid;
    Quaternion lidrot;

    public void SetGazedAt(bool gazedAt)
    {
        //GetComponent<Renderer>().material.color = gazedAt ? Color.black : Color.white;

        glow = GetComponent<ParticleSystem>();
        if (gazedAt == true)
        {
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
            GetComponent<Renderer>().material.color = Color.cyan;
            //lid.GetComponent<Renderer>().material.color = Color.cyan;
            if (!opening) { t = Time.time; }

            opening = true;
            Main.transform.position = Vector3.Lerp(Main.transform.position, transform.position, Time.deltaTime * speed); 
            if (open)
            {

            GlobalControl globs = Glob.GetComponent<GlobalControl>();
            string dir = globs.dir;
            if (globs.dir == "icHackPics")
            {
                globs.dir = "CodePDFs";
            }
            else if (globs.dir == "newPics")
            {
                globs.dir = "CodePDFs";
            }
            else if (globs.dir == "CodePDFs")
            {
                globs.dir = "newPics";
            }
            //SceneManager.LoadScene("Scene1");
        }



        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
            lid.GetComponent<Renderer>().material.color = Color.white;
            opening = false;

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
        GetComponent<Renderer>().material.color = Color.black;
        lid = this.transform.Find("Lid1").gameObject;
        GetComponent< Renderer > ().material.color = Color.white;
        lid.GetComponent<Renderer>().material.color = Color.white;
        Glob = GameObject.Find("Global");
        GlobalControl globs = Glob.GetComponent<GlobalControl>();
        GameObject words = this.transform.Find("Text").gameObject;
        if (globs.dir == "icHackPics") { words.GetComponent<TextMesh>().text = "CodePDFs"; }
        if (globs.dir == "newPics") { words.GetComponent<TextMesh>().text = "CodePDFs"; }
        if (globs.dir == "CodePDFs") { words.GetComponent<TextMesh>().text = "newPics"; }
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
            if (between.magnitude < 4)
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
        if (opening)
        {
            lid.transform.position += Vector3.up * speed * Time.deltaTime;
            if (Time.time - t > 1)
            {
                open = true;
            }
        }
        if (!opening && lid.transform.position.y > 1)
        {
            lid.transform.position -= Vector3.up * speed * Time.deltaTime;
        }

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
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }

        return tex;
    }
}
