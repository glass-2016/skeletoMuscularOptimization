﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	// anchor prefab for joint listArticulations only for debug
	public articulations anchorPrefab;
	public Dictionary<int, articulations> listArticulations;
	private Renderer render;
	public bool colliding = false;
	private Rigidbody rb;
	public Vector3 size;
	public bool started = false; 
	void Awake ()
	{
		listArticulations = new Dictionary<int, articulations>();
	}
	// Use this for initialization
	void Start () 
	{
		render = GetComponent<Renderer> ();
		size = GetComponent<Renderer> ().bounds.extents;
		rb = GetComponent<Rigidbody> ();
		rb.maxAngularVelocity = 4;
		rb.maxDepenetrationVelocity = 4;
		started = true;
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

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones")
		{
//			Debug.Log ("Some bones collision!!!");
			render.material.color = Color.red;
			colliding = true;
		}
		else if (other.tag == "collectibles")
			other.gameObject.SetActive (false);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones")
		{
			render.material.color = Color.white;
			colliding = false;
		}
	}
}
