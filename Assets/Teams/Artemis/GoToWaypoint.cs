using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class GoToWaypoint : Action
    {
        private ArtemisController _artemisController;

        public override void OnStart()
        {
            base.OnStart();
            
            _artemisController = GetComponent<ArtemisController>();
        }

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();
            
            _artemisController.goingToWaypoint = true;
            return TaskStatus.Success;
        }
    }
}