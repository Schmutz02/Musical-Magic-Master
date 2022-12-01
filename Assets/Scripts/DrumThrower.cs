using System;
using System.Collections;
using MMM.DialogSystem;
using UnityEngine;

namespace MMM
{
    public class DrumThrower : MonoBehaviour
    {
        public Collider2D DrumCollider;
        public GameObject DrumPrefab;
        public float Force;
        public float DrumLifetime;

        public AudioSource AudioSource;
        public AudioClip DrumSound;

        private GameObject _currentDrum;

        private bool _throwDrum;
        private void Update()
        {
            if (DialogManager.Instance.DialogActive)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _throwDrum = true;

                if (_currentDrum)
                {
                    StopAllCoroutines();
                    Destroy(_currentDrum);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_throwDrum)
                return;
            
            var drum = Instantiate(DrumPrefab, transform.position, Quaternion.identity);
            _currentDrum = drum;
            
            var mousePosWorld = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = (mousePosWorld - (Vector2)transform.position).normalized;
            drum.GetComponent<Rigidbody2D>().AddForce(dir * Force);
            _throwDrum = false;

            StartCoroutine(DestroyDrum());
        }

        private IEnumerator DestroyDrum()
        {
            yield return new WaitForSeconds(DrumLifetime);
            Destroy(_currentDrum);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var drum = other.transform.parent.GetComponentInParent<BouncyDrum>();
            if (!drum)
                return;

            other.gameObject.layer = 0;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.name == "Bounce")
                AudioSource.PlayOneShot(DrumSound);
        }
    }
}