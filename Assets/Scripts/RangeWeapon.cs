using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
   public Camera fpsCam;
   public float range = 100f;
   public float damage = 10f;
   public float impactForce = 200f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){

        	Shoot();
        }
    }

    void Shoot(){
    	RaycastHit hit;
    	if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){

    		Target target = hit.transform.GetComponent<Target>();

    		if(target != null){
    			target.TakeDamage(damage);
    		}

    		if(hit.rigidbody != null){

    			hit.rigidbody.AddForce(-hit.normal * impactForce);
    		}
    	}
    }
}
