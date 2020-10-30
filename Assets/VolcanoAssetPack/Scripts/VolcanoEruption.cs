using UnityEngine;
using TRKGeneric;

public delegate void EruptHandler();

namespace VISVolcano
{
    public class VolcanoEruption : MonoSingleton<VolcanoEruption>
    {
        public event EruptHandler Erupt;

        private bool isPaused;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Erupt != null)
                {
                    Erupt();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isPaused)
                {
                    Time.timeScale = 1;
                    isPaused = false;
                }
                else
                {
                    Time.timeScale = 0;
                    isPaused = true;
                }
            }
        }
    }
}
