using UnityEngine;

namespace VISVolcano
{
    public class RockLanding : MonoBehaviour
    {
        private void OnDisable()
        {
            RockLandingPool.Instance.ReturnToPool(this);
        }
    }
}
