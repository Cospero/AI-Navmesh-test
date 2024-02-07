using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Patrool : MonoBehaviour
{
    [SerializeField] private GameObject _pointsObject;
    
    private List<PointOb> points = new List<PointOb>();
    private NavMeshAgent agent;
    private float timer=0;

    private void Start() 
    {
        agent=GetComponent<NavMeshAgent>();
        _pointsObject.GetComponentsInChildren<PointOb>(true,points);
    }

    private void Update() 
    {
        if(agent.remainingDistance<=agent.stoppingDistance) {timer+=Time.deltaTime;}
        
        if(timer>2) 
        {
            StartPatrool();
            timer=0;
        }
        
        
    }

    private void StartPatrool()
    {
        agent.SetDestination(points[Random.Range(0,points.Count)].transform.position);

    }
}
