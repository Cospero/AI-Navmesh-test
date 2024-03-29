using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer;//layer with objects that obstruct the enemy view, like walls, for example
    public int VisionConeResolution = 120;//the vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be
    Mesh VisionConeMesh;
    [SerializeField] private GameObject target;
    [SerializeField] private Patrool _patrool;
    //[SerializeField] private GameObject _visionConeRotationOb;
    [SerializeField] private float _maxVerticalRotation; //Recommend value 35-40f
    private Transform _coneTransform;
    private bool _lastRotateUp;
    private bool _endRotate=true;
    MeshFilter MeshFilter_;
    //Create all of these variables, most of them are self explanatory, but for the ones that aren't i've added a comment to clue you in on what they do
    //for the ones that you dont understand dont worry, just follow along
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
        _coneTransform=this.gameObject.transform;
        
    }

    
    void Update()
    {
        DrawVisionCone();//calling the vision cone function everyframe just so the cone is updated every frame
        RotateVisionCone();

    }

    void DrawVisionCone()//this method creates the vision cone mesh
    {
	    
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
    	Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                if (hit.collider.gameObject== target)
                {
                    if(_patrool._isChasing==false)
                    {
                        _patrool.SwitchAgentStateByID(1);
                    }
                    
                }
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }



            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }

    private void RotateVisionCone()
    {
        if(_lastRotateUp)
        {
            _coneTransform.localRotation=Quaternion.Slerp(_coneTransform.localRotation, Quaternion.Euler(-_maxVerticalRotation,0,0), 25*Time.deltaTime);
            if (_coneTransform.localRotation.eulerAngles.x>=_maxVerticalRotation & _coneTransform.localRotation.eulerAngles.x<=360-_maxVerticalRotation+2)
            {
                _lastRotateUp=false;
            }
        }
        else if(!_lastRotateUp)
        {
            _coneTransform.localRotation=Quaternion.Slerp(_coneTransform.localRotation, Quaternion.Euler(_maxVerticalRotation,0,0), 25*Time.deltaTime);
            if (_coneTransform.localRotation.eulerAngles.x>=_maxVerticalRotation-3 & _coneTransform.localRotation.eulerAngles.x<100)
            {
                _lastRotateUp=true;
            }
        }
    }


}


