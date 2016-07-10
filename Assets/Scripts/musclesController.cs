using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	public Dictionary<articulations, ConfigurableJoint> joint;
	// anchor prefab for joint listArticulations only for debug
	public articulations anchorPrefab;
	private Rigidbody rb;
	public Dictionary<int, articulations> listArticulations;
	public bool colliding = false;

	// Use this for initialization
	void Start () 
	{
		joint = new Dictionary<articulations, ConfigurableJoint> ();
		listArticulations = new Dictionary<int, articulations>();
//		listMuscles = new Dictionary<int, muscle> ();
		rb = GetComponent<Rigidbody> ();
	}

	articulations checklistArticulations(Dictionary<int, articulations> list, Vector3 anchor, int current)
	{
		foreach (KeyValuePair<int, articulations> tmpAnchor in list)
		{
			Debug.Log ("currentAnchor, x = " + anchor.x + ", y = " + anchor.y + ", z = " + anchor.z);
			Debug.Log ("tmAnchor, x = " + tmpAnchor.Value.transform.position.x + ", y = " + tmpAnchor.Value.transform.position.y + ", z = " + tmpAnchor.Value.transform.position.z);
			if (Vector3.Distance(tmpAnchor.Value.transform.position, anchor) < 0.5)
				return (tmpAnchor.Value);
		}
		return (null);
	}

	public void addArticulation(articulations current)
	{
		if (!listArticulations.ContainsValue(current))
			listArticulations.Add (current.index, current);
	}

	// add muscle, set as controller and create joint if needed
	public articulations addAnchor(muscle tmp, Rigidbody rb, int index)
	{
		Vector3 anchorPos = (rb.transform.position - transform.position) / 2.0f;
		articulations tmpArticulation;
		if ((tmpArticulation = checklistArticulations (rb.gameObject.GetComponent<musclesController> ().listArticulations, anchorPos, index)) == null)
		{
			tmpArticulation = Instantiate (anchorPrefab, anchorPos, Quaternion.identity) as articulations;
			listArticulations.Add (index, tmpArticulation);
			tmpArticulation.addRigidBody (rb, tmp);
			tmpArticulation.setControllerAndIndex (this, index);
		}
		tmp.setAnchor (this.gameObject);
		return (tmpArticulation);
	}

	// Update is called once per frame
	void Update () 
	{
		foreach (KeyValuePair<int, articulations> tmpArticulation in listArticulations)
		{
			tmpArticulation.Value.gameObject.transform.position = transform.position + (tmpArticulation.Value.joint.connectedBody.transform.position - transform.position) / 2.0f;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
			Debug.Log ("Some bones collision!!!");
			colliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
