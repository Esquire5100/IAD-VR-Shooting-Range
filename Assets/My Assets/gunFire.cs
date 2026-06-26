using System.Collections;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] fireSFXs; //List of SFXs to play when firing
    public AudioSource audioSource;

    [Header("VFX")]
    public GameObject shootVFX;      //Muzzle flash prefab
    public Transform firePoint;      //Where the VFX should appear
    public GameObject hitVFX;

    public bool canFire = true;

    public static float maxDistance = 50f; // Maximum distance for the raycast
    private float toTarget;

    public void Fire()
    {
        canFire = false;
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        // Spawn VFX
        if (shootVFX != null && firePoint != null)
        {
            GameObject vfx = Instantiate(shootVFX, firePoint.position, firePoint.rotation);
            Destroy(vfx, 0.1f); // Destroy after 2 seconds (adjust as needed)
        }

        // Play a random fire sound effect
        int randomIndex = Random.Range(0, fireSFXs.Length);
        audioSource.PlayOneShot(fireSFXs[randomIndex]);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            maxDistance = hit.distance;
            toTarget = hit.distance;

            // Spawn hit effect at the impact point
            yield return new WaitForSeconds(0.2f);
            Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));

        }

        // Wait for the duration of the sound effect before allowing firing again
        yield return new WaitForSeconds(0.5f);
        canFire = true;
    }
}