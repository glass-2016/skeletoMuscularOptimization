using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	// anchor prefab for joint listArticulations only for debug
	public articulations anchorPrefab;
	public Dictionary<int, articulations> listArticulations;
	public bool colliding = false;
	private Rigidbody rb;
	public Vector3 size;

	// Use this for initialization
	void Start () 
	{
		listArticulations = new Dictionary<int, articulations>();
		size = GetComponent<Renderer> ().bounds.extents;
		rb = GetComponent<Rigidbody> ();
		rb.maxAngularVelocity = 4;
		rb.maxDepenetrationVelocity = 4;
	}

	articulations checklistArticulations(Dictionary<int, articulations> list, musclesController other)
	{
		foreach (KeyValuePair<int, articulations> tmpAnchor in list)
		{
			if ((this == tmpAnchor.Value.controllers[0] && other == tmpAnchor.Value.controllers[1])
				|| (this == tmpAnchor.Value.controllers[1] && other == tmpAnchor.Value.controllers[0]))
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
	public articulations addAnchor(muscle tmp, musclesController other, int index)
	{
		Vector3 anchorPos = Vector3.Lerp(other.transform.position, transform.position, 0.5f);
		articulations tmpArticulation;
		if ((tmpArticulation = checklistArticulations (listArticulations, other)) == null)
		{
			tmpArticulation = Instantiate (anchorPrefab, anchorPos, Quaternion.identity) as articulations;
			listArticulations.Add (index, tmpArticulation);
			tmpArticulation.addRigidBody (this, other, index);
		}
		tmpArticulation.addMuscle (tmp);
		return (tmpArticulation);
	}

	// Update is called once per frame
	void Update () 
	{
		if (rb.velocity.magnitude > 4)
			rb.velocity = rb.velocity.normalized * 4;
	}

	void OnCollisionStay(Collision other)
	{
		colliding = true;
	}

	void OnCollisionExit(Collision other)
	{
		colliding = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
//						Debug.Log ("Some bones collision!!!");
			colliding = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
//			Debug.Log ("Some bones collision!!!");
			colliding = true;
		}
		else if (other.tag == "collectibles")
			other.gameObject.SetActive (false);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
