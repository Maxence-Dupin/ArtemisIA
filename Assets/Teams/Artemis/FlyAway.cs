using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Artemis
{
    public class FlyAway : Action
    {
        public SpaceShip _spaceship;
        public Vector2 _target;

        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();

            while (_spaceship.Position != _target)
            {
                return TaskStatus.Running;
            }
            
            return TaskStatus.Success;
        }
    }
}