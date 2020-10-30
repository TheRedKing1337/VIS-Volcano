using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VISVolcano
{
    public class SmokePlume : MonoBehaviour
    {
        public GameObject smokePlumePrefab;

        private ParticleSystem smokePlume;
        private void OnEnable()
        {
            VolcanoEruption.Instance.Erupt += StartSmokePlume;
        }
        private void OnDisable()
        {
            VolcanoEruption.Instance.Erupt -= StartSmokePlume;
        }
        private void StartSmokePlume()
        {
            if (smokePlume == null)
            {
                smokePlume = Instantiate(smokePlumePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                smokePlume.transform.SetParent(transform);
            }

            smokePlume.Play();
        }
    }
}
