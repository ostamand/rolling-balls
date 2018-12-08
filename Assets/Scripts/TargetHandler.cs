using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour {

    #region Public Fields

    public ParticleSystem agentFX;
    public ParticleSystem playerFX;

    #endregion

    #region Private Fields

    private Renderer _renderer;
    private GameController _game;
    private bool _moving = false;
    private MeshCollider _collider;

    #endregion

    void Start ()
    {
        _game = FindObjectOfType<GameController>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<MeshCollider>();
    }
	
	void Update ()
    {
		
	}

    public void UpdatePosition(Vector3 from, TypeOf update)
    {
        float distanceToTarget = Vector3.Distance(from, this.transform.position);
        // reached target
        if (!_moving && distanceToTarget < 1.3f)
        {
            _game.Hit(update);
            NewPosition(update);
        }
    }
        
    // TODO disable agent and player
    public void NewPosition(TypeOf update)
    {
        _moving = true;
        _collider.enabled = false;
        if(update == TypeOf.Agent){ agentFX.Play(); }
        if (update == TypeOf.Player) { playerFX.Play(); }
        Invoke("Display", 0.2f);
    }

    private void Display()
    {
        // move the target to a new spot
        this.transform.position = new Vector3(Random.value * 8 - 4,
                                      0.0f,
                                      Random.value * 8 - 4);
        _moving = false;
        _collider.enabled = true;
        
    }



}
