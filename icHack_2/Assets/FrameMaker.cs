using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEditor;

public class FrameMaker : MonoBehaviour {

    public GameObject frame;
    public GameObject hotAirBalloon;
    GameObject hot;
    Quaternion init180;
    public GameObject[] frames;
    GameObject Glob;
    Quaternion initToStart;
    public float speed;

    // Use this for initialization

    public void setCoordinates(float[] xValues, float[] zValues, int numberOfEntries, float radius, float distanceToAny)
    {

        xValues[numberOfEntries] = Random.Range(-radius, radius);
        zValues[numberOfEntries] = Mathf.Sign(Random.Range(-1, 1)) * Mathf.Sqrt(radius * radius - (xValues[numberOfEntries]) * (xValues[numberOfEntries]));
        float distance;
        for (int j = 0; j < numberOfEntries; j++)
        {
            distance = Mathf.Sqrt((xValues[numberOfEntries] - xValues[j]) * (xValues[numberOfEntries] - xValues[j]) + (zValues[numberOfEntries] - zValues[j]) * (zValues[numberOfEntries] - zValues[j]));
            if (distance < distanceToAny)
            {
                setCoordinates(xValues, zValues, numberOfEntries, radius + 0.5f, distanceToAny);
				return;
            }
        }
    }

    void Start () {

        Glob = GameObject.Find("Global");
        GlobalControl globs = Glob.GetComponent<GlobalControl>();
        string dir = globs.dir;
        print(globs.dir);
        /*string[] folders = AssetDatabase.GetSubFolders(@"Assets/Resources");
        print(folders.Length);
        for (int i = 0; i < folders.Length; i++)
        {
            print(folders[i]);
        }*/
        Object[] textures = Resources.LoadAll(dir, typeof(Texture2D));
        int numberOfPictures = textures.Length;
        //print(numberOfPictures);
        init180.eulerAngles = new Vector3(0, 180, 0);
        frames = new GameObject[numberOfPictures];

        float[] xValues = new float[numberOfPictures];
        float[] zValues = new float[numberOfPictures];

        float radius = 25;
        float distanceToAny = 20;

        xValues[0] = 5;
        zValues[0] = 5;

        for (int i = 1; i < numberOfPictures; i++)
        {
            setCoordinates(xValues, zValues, i, 2 * (radius * Mathf.Exp(-0.5f * (i - 1))), distanceToAny);
        }
        for (int i = 0; i < numberOfPictures; i++)
        {
            //print(xValues[i]);
            //print(zValues[i]);
        }

        for (int i = 0; i < numberOfPictures; i++)
        {
            Vector3 initLoc = new Vector3(-xValues[i], 0, -zValues[i]);
            initToStart.SetLookRotation(initLoc);
            
            if (i==6||i==7)
            {
                initToStart.eulerAngles += new Vector3(0, 0,90);
            }
            else
            {
                initToStart.eulerAngles += new Vector3(0, 0, 0);
            }

            frames[i] = Instantiate(frame, new Vector3(xValues[i], Terrain.activeTerrain.SampleHeight(new Vector3(xValues[i],0,zValues[i]))+4.0f, zValues[i]), initToStart) as GameObject;
            var rend = frames[i].GetComponent<Renderer>();
            rend.material.mainTexture = textures[i] as Texture;
        }
        
        hot=Instantiate(hotAirBalloon, new Vector3(-250, 250, 100), Quaternion.LookRotation(new Vector3(90,0,0)))as GameObject;
	}   
	
	// Update is called once per frame
	void Update () {
        GlobalControl globs = Glob.GetComponent<GlobalControl>();
        if (globs.dir == "")
        {
            globs.dir = "icHackPics";
            SceneManager.LoadScene("Scene1");
        }
        hot.transform.position += Vector3.up*speed;

    }

}
