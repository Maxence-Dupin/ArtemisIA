using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class GoToWaypoint : Action
    {
        private ArtemisController _artemisController;
        private Vector2 _target;
        private SpaceShipView _spaceShipView;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
            _spaceShipView = GameManager.Instance.GetSpaceShipForController(_artemisController).view;
            _artemisController.goingToWaypoint = true;
        }

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            if (_spaceShipView.Position != _target)
            {
                return TaskStatus.Running;
            }

            _artemisController.goingToWaypoint = false;
            return TaskStatus.Success;
        }
    }
}