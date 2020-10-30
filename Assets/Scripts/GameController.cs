using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRKGeneric;

public class GameController : MonoSingleton<GameController>
{
    public float shockwaveStrength = 10;

    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem rocks;
    [SerializeField] private GameObject dustCloud;
    [SerializeField] private ParticleSystem shockwaveParticle;
    [SerializeField] private GameObject volcano;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private WindZone windZone;

    private const float speedOfSound = 343;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EruptVolcano());
        }
    }

    IEnumerator EruptVolcano()
    {
        //later use anim to control sequence of events

        Debug.Log("Starting small smoke sequence");
        //start small smoke

        //wait
        yield return new WaitForSeconds(1f);
        Debug.Log("Starting explosion sequence");

        //start explosion
        //start effect that blinds for short moment     
        //todo
        //start large coloured smoke
        smoke.Play();
        //start throw one burst of rocks with fire trails
        rocks.Play();
        //start some sort of lava flow
        //todo
        //maybe a visual shockwave that pushes trees down as it moves towards camera (dunno how, moving windzone?)
        float distance = (volcano.transform.position - playerCamera.transform.position).magnitude;
        //StartCoroutine(Shockwave(distance));
        //shockwaveParticle.Play();
        StartCoroutine(MoveDustClouds(distance));
        //wait
        float timeToReachCamera = distance / speedOfSound;
        yield return new WaitForSeconds(timeToReachCamera);
        Debug.Log("Starting shockwave hit sequence");

        StartCoroutine(HitCamera());
        //play sound        
        //start rumble of shockwave
    }
    IEnumerator Shockwave(float distance)
    {       
        windZone.radius = 0;
        windZone.windMain = shockwaveStrength;
        while (windZone.radius < distance + 300)
        {
            windZone.radius += speedOfSound * Time.deltaTime;
            yield return null;
        }
        while (windZone.windMain > 1)
        {
            windZone.windMain -= (shockwaveStrength / 5) * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MoveDustClouds(float distance)
    {
        //yield return new WaitForSeconds(0.1f);
        int numParticles = 72;
        float rotationValue = 360 / numParticles;
        Vector3 rotationPreset = new Vector3(-90, -90, 0);
        //enable all dust clouds (refactor to generate 36/72 in each another direction)
        ParticleSystem[] dustClouds = new ParticleSystem[numParticles];
        for(int i=0;i<numParticles;i++){
            dustClouds[i] = Instantiate(dustCloud, transform.position, Quaternion.Euler(rotationPreset)).GetComponent<ParticleSystem>();
            dustClouds[i].transform.Rotate(Vector3.forward * i * rotationValue);
        }
        //while not reached camera
        while (dustClouds[0].transform.position.magnitude < distance *2)
        {
            float lerpValue = Mathf.Lerp(1, 0, dustClouds[0].transform.position.magnitude/(distance*2));
            //foreach dust cloud
            for (int i = 0; i < dustClouds.Length; i++)
            {
                //move forward
                dustClouds[i].transform.Translate(dustClouds[i].transform.forward * speedOfSound * Time.deltaTime);
                //raycast for height of terrain
                RaycastHit height;
                Physics.Raycast(dustClouds[i].transform.position + (Vector3.up * 100), Vector3.down, out height);
                //move to height of terrain
                dustClouds[i].transform.position = new Vector3(dustClouds[i].transform.position.x, height.point.y + 2, dustClouds[i].transform.position.z);
                //scale down
                dustClouds[i].transform.localScale = Vector3.one * lerpValue;
            }
            yield return null;
        }
            //remove the dust clouds (needs a fading effect)
            for (int i = 0; i < dustClouds.Length; i++)
        {
            Destroy(dustClouds[i].gameObject);
        }
    } 
    IEnumerator HitCamera()
    {
        Camera camera = playerCamera.GetComponent<Camera>();
        StartCoroutine(Shake(1f, 4, playerCamera.transform));

        //dit is dom
        //float timer = Time.time + 0.2f;
        //while(timer > Time.time)
        //{
        //    camera.fieldOfView += 50 * Time.deltaTime;
        //    yield return null;
        //}
        //timer = Time.time + 1;
        //while (camera.fieldOfView > 60)
        //{
        //    camera.fieldOfView -= 5 * Time.deltaTime;
        //    yield return null;
        //}  
        yield return null;     
    }
    public IEnumerator Shake(float duration, float magnitude, Transform camera)
    {
        Vector3 originalPos = camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            //random shake
            float z = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camera.transform.localPosition = new Vector3(originalPos.x, originalPos.y+y, originalPos.z+z);

            //voor timer
            elapsed += Time.deltaTime;

            yield return null;
        }
        //terug naar originele plek
        camera.transform.localPosition = originalPos;
    }
}
