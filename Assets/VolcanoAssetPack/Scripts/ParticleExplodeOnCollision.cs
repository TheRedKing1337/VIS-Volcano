using System.Collections.Generic;
using UnityEngine;

namespace VISVolcano
{
    public class ParticleExplodeOnCollision : MonoBehaviour
    {
        public ParticleSystem explosion;

        private ParticleSystem part;
        private List<ParticleCollisionEvent> collisionEvents;

        void Start()
        {
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }
        private void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            int i = 0;

            while (i < numCollisionEvents)
            {
                Vector3 pos = collisionEvents[i].intersection;

                RockLanding rl = RockLandingPool.Instance.Get();
                rl.transform.position = pos;
                rl.gameObject.SetActive(true);

                i++;
            }
        }
    }
}
