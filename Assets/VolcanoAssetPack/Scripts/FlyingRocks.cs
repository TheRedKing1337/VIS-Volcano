using UnityEngine;

namespace VISVolcano
{
    public class FlyingRocks : MonoBehaviour
    {
        public GameObject flyingRocksPrefab;

        private ParticleSystem flyingRocks;
        private void OnEnable()
        {
            VolcanoEruption.Instance.Erupt += StartFlyingRocks;
        }
        private void OnDisable()
        {
            VolcanoEruption.Instance.Erupt -= StartFlyingRocks;
        }

        private void StartFlyingRocks()
        {
            if (flyingRocks == null)
            {
                flyingRocks = Instantiate(flyingRocksPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                flyingRocks.transform.SetParent(transform);

                GameObject[] objsWithGroundTag = GameObject.FindGameObjectsWithTag("Ground");
                for (int i = 0; i < objsWithGroundTag.Length; i++)
                {
                    flyingRocks.collision.SetPlane(i, objsWithGroundTag[i].transform);
                }
            }
            flyingRocks.Play();
        }
    }
}
