using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMM
{
    public class Bobbing : MonoBehaviour
    {
        public float Frequency;
        public float Amplitude;
        public bool RandomStartPos;

        private Vector2 _startPos;
        private float _startOffset;

        private void Start()
        {
            _startPos = transform.position;
            if (RandomStartPos)
                _startOffset = Random.Range(-Frequency, Frequency);
        }

        private void Update()
        {
            var offset = new Vector2(0, Mathf.Sin(Time.time * Frequency + _startOffset) * Amplitude);
            transform.position = _startPos + offset;
        }
    }
}
