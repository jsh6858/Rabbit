using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDugu : MonoBehaviour
{
    float fTime = 0.0f;
    public GameObject Prefab;
	void Start () {
		
	}
	
	// Update is called once per frame
	public IEnumerator Dugu()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject TempObject = Instantiate(Prefab);
        TempObject.transform.position =  new Vector3(Random.Range(-1.5f,0.0f),Random.Range(-3.5f,3.0f),0.0f);
        TempObject.transform.rotation =  Quaternion.Euler(Vector3.forward*Random.Range(-30.0f,30.0f));

        yield return new WaitForSeconds(0.3f);

        GameObject TempObject2 = Instantiate(Prefab);
        TempObject2.transform.position =  new Vector3(Random.Range(0.0f,1.5f),Random.Range(-3.5f,3.0f),0.0f);
        TempObject2.transform.rotation =  Quaternion.Euler(Vector3.forward*Random.Range(-30.0f,30.0f));

        yield return new WaitForSeconds(0.2f);

        GameObject TempObject3 = Instantiate(Prefab);
        TempObject3.transform.position =  new Vector3(Random.Range(-1.5f,0.0f),Random.Range(-3.5f,3.0f),0.0f);
        TempObject3.transform.rotation =  Quaternion.Euler(Vector3.forward*Random.Range(-30.0f,30.0f));

    }
}
