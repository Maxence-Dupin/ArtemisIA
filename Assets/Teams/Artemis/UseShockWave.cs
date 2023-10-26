using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class UseShockWave : Action
    {
        public ArtemisController _artemisController;
        public Vector2 _target;

        private SpaceShipView _spaceShipView;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _spaceShipView = GameManager.Instance.GetSpaceShipForController(_artemisController).view;
            _artemisController.GoingToWaypoint = true;
        }

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            return TaskStatus.Success;
        }
    }
}