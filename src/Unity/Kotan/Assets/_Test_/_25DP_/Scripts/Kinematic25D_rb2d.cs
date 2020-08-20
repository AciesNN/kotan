using UnityEngine;

//Wrap up rigidbody2D behavior in 2.5D world
[RequireComponent(typeof(Rigidbody))]
public class Kinematic25D_rb2d : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Rigidbody2D rb { get { if (_rb == null) { _rb = GetComponent<Rigidbody2D>(); } return _rb; } }

    public float Depth { private set; get; }
    public bool IsFlying { private set; get; }
    private float _lastTransformY;
    private Transform _transform;

    public bool IsMoveHorizontaly { get => rb.isKinematic; set => rb.isKinematic = value; }

    private void Awake()
    {
        rb.isKinematic = true;
        _transform = transform;
        _lastTransformY = _transform.position.y;
    }

    private void LateUpdate()
    {
        var y = _transform.position.y;
        if (IsMoveHorizontaly && !IsFlying)
        {
            var delta = y - _lastTransformY;
            if (delta > float.Epsilon || delta < -float.Epsilon)
            {
                //TODO - floating after fly
                Depth += delta;
            }
        }
        _lastTransformY = y;
    }

    public void Move(Vector3 dir, Vector2 speed)
    {
        if (!IsMoveHorizontaly)
        {
            return;
        }

        rb.velocity = dir * speed;
        IsFlying = false;
    }

    public void Fly(Vector3 dir, float speed)
    {
        if (!IsMoveHorizontaly)
        {
            return;
        }

        if (dir.sqrMagnitude < float.Epsilon)
        {
            return;
        }

        if (speed < float.Epsilon)
        {
            return;
        }

        rb.velocity = dir * speed;
        IsFlying = true;
    }

    public void Jump(float force)
    {
        if (!IsMoveHorizontaly)
        {
            return;
        }

        IsFlying = false;
        IsMoveHorizontaly = false;
        TurnOffVerticalVelocity();
        AddForce(Vector2.up * force);
    }

    public void Strike(Vector2 dir, float force)
    {
        if (IsMoveHorizontaly)
        {
            return;
        }

        AddForce(dir * force);
    }

    private void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void TurnOffVerticalVelocity()
    {
        rb.velocity *= Vector2.right;
    }
}