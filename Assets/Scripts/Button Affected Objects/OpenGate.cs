using System;
using Buttons;
using UnityEngine;

namespace Button_Affected_Objects
{
    public class OpenGate : ButtonObject
    {
        [SerializeField] private Directions directions;
        [SerializeField] private float factor;
        [SerializeField] private bool smooth = false;
        
        
        private Transform _transform;
        private Vector3 _newPosition;

        private bool once;
        
        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            var position = _transform.position;
            _newPosition = directions switch
            {
                Directions.Up => new Vector3(position.x, position.y + factor, position.z),
                Directions.Down => new Vector3(position.x, position.y - factor, position.z),
                Directions.Right => new Vector3(position.x + factor, position.y, position.z),
                Directions.Left => new Vector3(position.x - factor, position.y, position.z),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Perform()
        {
            if (!smooth)
                LeanTween.move(_transform.gameObject, _newPosition, 1f);
            else
                LeanTween.move(_transform.gameObject, _newPosition, 1f).setEaseInOutQuad();
        }
        
        private enum Directions
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}
