using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace VISVolcano
{
    public class ShockwaveScript : MonoBehaviour
    {
        public int numParticles = 72;
        public float distance = 1000;
        public GameObject shockwaveParticle;

        private void OnEnable()
        {
            VolcanoEruption.Instance.Erupt += StartShockwave;
        }
        private void OnDisable()
        {
            VolcanoEruption.Instance.Erupt -= StartShockwave;
        }
        private void StartShockwave()
        {
            StartCoroutine(Shockwave());
        }
        private IEnumerator Shockwave()
        {
            float rotationValue = 360 / numParticles;
            Vector3 rotationPreset = new Vector3(-90, -90, 0);

            //Instantiate wanted amount of dust clouds
            Transform[] particleTransforms = new Transform[numParticles];

            for (int i = 0; i < numParticles; i++)
            {
                particleTransforms[i] = Instantiate(shockwaveParticle, transform.position, Quaternion.Euler(rotationPreset)).GetComponent<Transform>();
                particleTransforms[i].Rotate(Vector3.forward * i * rotationValue);
                particleTransforms[i].SetParent(transform);
            }

            //while not reached final distance
            while (particleTransforms[0].transform.localPosition.magnitude < distance)
            {
                //lerpValue for shrinking particles as they get further away from sources
                float lerpValue = Mathf.Lerp(1, 0, particleTransforms[0].transform.position.magnitude / (distance));

                //foreach particle because you cant raycast in threads RIP performance
                for (int i = 0; i < particleTransforms.Length; i++)
                {
                    particleTransforms[i].Translate(particleTransforms[i].forward * 343 * Time.deltaTime);
                    //scale down
                    particleTransforms[i].localScale = Vector3.one * lerpValue;
                    //raycast for height of terrain
                    RaycastHit height;
                    Physics.Raycast(particleTransforms[i].transform.position + (Vector3.up * 100), Vector3.down, out height);
                    //move to height of terrain
                    particleTransforms[i].transform.position = new Vector3(particleTransforms[i].transform.position.x, height.point.y + 2, particleTransforms[i].transform.position.z);
                }
                yield return null;
            }

            //remove the dust clouds when done (maybe pool if multiple shockwaves are wanted)
            for (int i = 0; i < particleTransforms.Length; i++)
            {
                Destroy(particleTransforms[i].gameObject);
            }
        }
    }
}
