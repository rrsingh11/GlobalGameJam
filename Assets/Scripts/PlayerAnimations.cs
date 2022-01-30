using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private DashController _dashController;
    [SerializeField] private GameObject blackParticle;
    [SerializeField] private GameObject whiteParticle;
    
    private Animator _player;
    private PlayerController _playerController;

    private Transform loc1;
    private Transform loc2;
    
    private void Start()
    {
        _player = GetComponent<Animator>();
        
        var parent = transform.parent;
        _playerController = parent.GetComponent<PlayerController>();
        loc1 = parent.GetChild(1);
        loc2 = parent.GetChild(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerController.onGround)
        {
            _player.Play("Jump", -1, 0f);
            if (loc1.position.y > transform.parent.position.y)
                Instantiate(blackParticle, loc2.position, Quaternion.identity);
            else
                Instantiate(whiteParticle, loc1.position, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.X) && _playerController.moveHorizontal != 0f && _dashController.canDash)
            _player.Play("Dash", -1, 0f);
    }
}
