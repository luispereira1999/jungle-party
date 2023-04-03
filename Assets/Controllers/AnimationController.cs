using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool isWalking;
    private Animator animator;
    private float walkTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            walkTime += Time.deltaTime;
            animator.Play("Walk", -1, walkTime);
        }
        else
        {
            animator.Play("Idle");
        }
    }
}