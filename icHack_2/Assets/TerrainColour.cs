using UnityEngine;
using System.Collections;

public class TerrainColour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.green;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
