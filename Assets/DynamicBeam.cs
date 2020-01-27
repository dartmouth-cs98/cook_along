using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

// using magic leap developer portal tutorial found at: https://developer.magicleap.com/learn/guides/unity-overview
public class DynamicBeam : MonoBehaviour
{
    LineRenderer beamLine;
    public Transform startPos;
    public Transform endPos;
    

    // Start is called before the first frame update
    void Start()
    {
        beamLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit))
        // {
        //     beamLine.useWorldSpace = true;
        //     beamLine.SetPosition(0, transform.position);
        //     beamLine.SetPosition(1, hit.point);
        // }
        // else
        // {
        //     beamLine.useWorldSpace = false;
        //     beamLine.SetPosition(0, transform.position);
        //     beamLine.SetPosition(1, Vector3.forward * 5);
        // }
        
        beamLine.SetPosition(0, transform.position);
        beamLine.SetPosition(1, transform.position + (Vector3.forward * 0.3f));
    }
}
