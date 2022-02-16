using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Game.Code.Logic.Enemies
{
    public class WaypointNetwork
    {
        public event Action LastPointReached;
        
        private List<Transform> _waypointList;

        private int _currentIndex = 0;

        public WaypointNetwork(List<Transform> waypointList) => 
            _waypointList = waypointList;

        public Transform GetNextWaypoint()
        {
            if (_waypointList.Count == 0)
                return null;

            Transform nextWaypoint = null;
            int nextIndex = _currentIndex + 1;

            if (nextIndex <= _waypointList.Count)
            {
                nextWaypoint = _waypointList[_currentIndex];
                _currentIndex = nextIndex;

                if (_currentIndex == _waypointList.Count)
                {
                    _currentIndex = 0;
                    LastPointReached?.Invoke();
                }
            }

            return nextWaypoint;
        }

        public Transform GetRandomWaypoint()
        {
            if (_waypointList.Count == 0)
                return null;

            int randomIndex = Random.Range(0, _waypointList.Count);
            return _waypointList[randomIndex];
        }
    }
}