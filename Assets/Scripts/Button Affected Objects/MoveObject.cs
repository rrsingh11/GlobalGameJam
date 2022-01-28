using System;
using Buttons;
using UnityEngine;

namespace Button_Affected_Objects
{
    public class MoveObject : ButtonObject
    {
        [SerializeField] private Transform loc1;
        [SerializeField] private Transform loc2;

        private bool _switchLocation;

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, !_switchLocation ? loc2.position : loc1.position, 1f * Time.deltaTime);
        }

        public override void Perform()
        {
            _switchLocation = !_switchLocation;
        }
    }
}
