using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGate : MonoBehaviour
{
    public MeshRenderer BHmesh;
    public MeshRenderer redMesh;
    public ParticleSystem parSys;

    void Start() 
    {
        Hide();
    }

    public void Show()
    {
        BHmesh.enabled = true;
        redMesh.enabled = true;
        var color = parSys.main;
        color.startColor = Color.black;
    }

    public void Hide()
    {
        BHmesh.enabled = false;
        redMesh.enabled = false;
        var color = parSys.main;
        color.startColor = new Color(0,0,0,0);
    }

}
