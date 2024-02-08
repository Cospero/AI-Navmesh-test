using System;
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
    private Transform _currentTarget;
    private Transform _lastPatroolpoint;

    private void Start() 
    {
        agent=GetComponent<NavMeshAgent>();
        _pointsObject.GetComponentsInChildren<PointOb>(true,points);
        _currentTarget=points[0].transform;
    }

    private void Update() 
    {
        if(agent.remainingDistance<=agent.stoppingDistance) {timer+=Time.deltaTime; Debug.Log(timer);}
        
        if(timer>2 & agent.remainingDistance<=agent.stoppingDistance) 
        {
            Startpatrool();
        }

        if (_lastPatroolpoint==_currentTarget)
        {
            Startpatrool();
        }
        
        
    }

    IEnumerator SetDestinationToPoint()
    {
        while(true)
        {

            yield return new WaitForSeconds(0.1f);
            Debug.Log("Go to position");
            agent.SetDestination(_currentTarget.position);
        }
    }

    private void SetPatroolDestination()
    {
        _lastPatroolpoint=_currentTarget;
        _currentTarget=points[UnityEngine.Random.Range(0,points.Count)].transform;
        
        StartCoroutine("SetDestinationToPoint");
        //agent.SetDestination(points[Random.Range(0,points.Count)].transform.position);

    }

    private void Startpatrool()
    {
        StopCoroutine("SetDestinationToPoint");
        SetPatroolDestination();
        timer=0;
    }

}
