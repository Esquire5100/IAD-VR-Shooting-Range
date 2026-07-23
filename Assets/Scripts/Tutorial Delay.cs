using UnityEngine;
using System.Collections;

public class TutorialDelay : MonoBehaviour
{
    public GameObject bot1;
    public GameObject bot2;
    public GameObject bot3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bot1.SetActive(false);
        bot2.SetActive(false);
        bot3.SetActive(false);
        StartCoroutine(ActivateBots());
    }

    public IEnumerator ActivateBots()
    {
        yield return new WaitForSeconds(10f);
        bot1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bot2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bot3.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
