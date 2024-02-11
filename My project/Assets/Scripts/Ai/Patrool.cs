using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Patrool : MonoBehaviour
{
    public LayerMask VisionObstructingLayer;
    public bool _isPatrooling {private set;get;}
    public bool _isChasing {private set;get;}
    [SerializeField] private GameObject _pointsObject;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _lastSeenPlayerPosition;
    [SerializeField] private float _attackRange=1.5f;
    private List<PointOb> points = new List<PointOb>();
    private NavMeshAgent agent;
    private float timer=0;
    private Transform _currentTarget;
    public bool _startChasing {private set;get;}
    private Transform _lastPatroolpoint;

    private void Start() 
    {
        agent=GetComponent<NavMeshAgent>();
        _pointsObject.GetComponentsInChildren<PointOb>(true,points);
        _currentTarget=points[0].transform;
        _isPatrooling=true;
        _startChasing=true;
    }

    private void Update() 
    {
        if(_isPatrooling)
        {
            Patrooling();
        }
        else if(_isChasing)
        {
            Chasing();
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

    private void Chasing()
    {   if(_startChasing)
        {
            _startChasing=false;
            Debug.Log("Start chasing");
            StopCoroutine("SetDestinationToPoint");
            _currentTarget=_player.transform;
            StartCoroutine("SetDestinationToPoint");
            if(agent.remainingDistance<=_attackRange)
            {
                Debug.Log("Player in attack range!");
            }
        }

        if(Physics.Raycast(transform.position, _player.transform.position-transform.position, out RaycastHit hit, 40f, VisionObstructingLayer))
        {
            Debug.Log("see player");
            if (hit.collider.gameObject.tag=="Default")
            {
                StopCoroutine("SetDestinationToPoint");
                Debug.Log("dont see player");
                _lastSeenPlayerPosition.position=_player.transform.position;
                _currentTarget=_lastSeenPlayerPosition;
                StartCoroutine("SetDestinationToPoint");
            }
            
        }
        else 
        {
            Debug.Log("dont see anything");
            _lastSeenPlayerPosition.position=_player.transform.position;
            _currentTarget=_lastSeenPlayerPosition;
        }
        
        
    }
    private void Patrooling()
    {
        if(agent.remainingDistance<=agent.stoppingDistance) {timer+=Time.deltaTime;}
        
            if((timer>2 & agent.remainingDistance<=agent.stoppingDistance)||(_lastPatroolpoint==_currentTarget))
            {
                StopCoroutine("SetDestinationToPoint");
                SetPatroolDestination();
                timer=0;
            }
    }
    public void SwitchAgentStateByID(int id)
    {
        if(id == 0)
        {
            _isPatrooling = true;
            _isChasing = false;
        }
        else if(id == 1)
        {
            _isChasing = true;
            _startChasing=true;
            _isPatrooling = false;
        }
    }
}
