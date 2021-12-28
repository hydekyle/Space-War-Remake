using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AspaBoss : MonoBehaviour
{
    public Transform aspa1, aspa2;
    public Stats stats;
    public SpriteRenderer aspa1Renderer, aspa2Renderer;
    Color aspa1Color, aspa2Color;
    public float rotationSpeed = 30f;
    public Animator animator;
    public CircleCollider2D weakPoint;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        aspa1Color = aspa1Renderer.color;
        aspa2Color = aspa2Renderer.color;
        PlayFirstAnimation();
    }

    async void PlayFirstAnimation()
    {
        await UniTask.WaitUntil(() => aspa1.transform.localPosition == Vector3.zero);
        await UniTask.WaitUntil(() => aspa2.transform.localPosition == Vector3.zero);
        animator.Play("RedEyeOn");
        var animTimeSeconds = (double)animator.GetCurrentAnimatorStateInfo(0).length;
        await UniTask.Delay(System.TimeSpan.FromSeconds(animTimeSeconds));
        weakPoint.enabled = true;
    }

    private void Update()
    {
        AspasAnimation();
    }

    void AspasAnimation()
    {
        aspa1.transform.localPosition = Vector3.MoveTowards(aspa1.transform.localPosition, Vector3.zero, Time.deltaTime * rotationSpeed / 3);
        aspa2.transform.localPosition = Vector3.MoveTowards(aspa2.transform.localPosition, Vector3.zero, Time.deltaTime * rotationSpeed / 3);
        aspa1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        aspa2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
        aspa1Renderer.color = Color.Lerp(aspa1Renderer.color, aspa1Color, Time.deltaTime * 10);
        aspa2Renderer.color = Color.Lerp(aspa2Renderer.color, aspa2Color, Time.deltaTime * 10);
    }

    void Hit()
    {
        stats.health -= GameManager.Instance.player.ship.stats.bulletDamage;
        aspa1Renderer.color = aspa2Renderer.color = Color.red;
        if (stats.health <= 0) Die();
    }

    void Die()
    {
        weakPoint.enabled = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            Hit();
        }
    }
}
