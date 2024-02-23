using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    public float minVelForWalkAnim = 0.1f;

    public float mag;

    private Animator _animator;
    private PlayerInput ctrl;

    // Start is called before the first frame update
    private void Start()
    {
        ctrl = GetComponent<PlayerInput>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        mag = ctrl.moveDir.magnitude;
        var isWalking = mag > minVelForWalkAnim;
        _animator.SetBool(IsWalking, isWalking);
    }
}