using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    [Header("시야 설정")]
    public float viewAngle = 120f;             // 시야각
    public float viewDistance = 10f;           // 시야 거리

    [Header("레이어 설정")]
    public LayerMask targetMask;               // 플레이어 레이어
    public LayerMask obstacleMask;             // 벽/장애물 레이어

    private MonsterController controller;
    private Transform player;

    void Start()
    {
        controller = GetComponentInParent<MonsterController>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null || controller.IsChasing())
            return;

        if (IsPlayerInSight())
        {
            controller.OnPlayerSeen(player);
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        // 시야 범위 및 각도 확인
        if (distToPlayer > viewDistance || angleToPlayer > viewAngle / 2f)
            return false;

        // 장애물 레이캐스트 검사
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, viewDistance, targetMask | obstacleMask))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // 에디터에서 시야 확인용 Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftDir = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, leftDir * viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up, rightDir * viewDistance);
    }
}
