using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coco
{
    public struct BoolTracker
    {
        private bool _value;
        private double _lastTimeChanged;

        public bool Value
        {
            get
            { 
                return _value; 
            }
            set 
            {
                if (_value != value)
                {
                    _value = value;
                    _lastTimeChanged = Time.timeAsDouble;
                }
            }
        }

        public double LastTimeChanged => _lastTimeChanged;

        public bool GetValueWithTolerance(double tolerance)
        {
            return _value || (!_value && _lastTimeChanged + tolerance >= Time.timeAsDouble);
        }

        public void Invalidate()
        {
            _lastTimeChanged = double.NegativeInfinity;
        }

        public static BoolTracker Create(bool defaultValue = false) => new BoolTracker
        {
            _value = defaultValue,
            _lastTimeChanged = double.NegativeInfinity
        };
    }
}