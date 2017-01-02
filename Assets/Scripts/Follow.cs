using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Follow : MonoBehaviour
{
    public Transform TargetObj = null;
    public float FollowDis = 4;

    protected Animator avatar;
    protected CharacterController controller;
    private float SpeedDampTime = .25f;
    private float DirectionDampTime = .25f;

    void Start()
    {
        avatar = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        avatar.speed = 1 + UnityEngine.Random.Range(-0.4f, 0.4f);
    }

    void Update()
    {
        if (avatar && TargetObj)
        {
            if (NeedFollow())
            {
                avatar.SetFloat("Speed", 1, SpeedDampTime, Time.deltaTime);

                Vector3 curentDir = avatar.rootRotation * Vector3.forward;
                Vector3 wantedDir = (TargetObj.position - avatar.rootPosition).normalized;

                if (Vector3.Dot(curentDir, wantedDir) > 0)
                {
                    avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y, DirectionDampTime, Time.deltaTime);
                }
                else
                {
                    avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
                }
            }
            else
            {
                avatar.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
            }
        }
    }

    private bool NeedFollow()
    {
        Vector3 v1 = TargetObj.position;
        v1.y = 0;
        Vector3 v2 = avatar.rootPosition;
        v2.y = 0;
        return Vector3.Distance(v1, v2) > FollowDis;
    }

    void OnAnimatorMove()
    {
        controller.Move(avatar.deltaPosition);
        transform.rotation = avatar.rootRotation;
    }
}
