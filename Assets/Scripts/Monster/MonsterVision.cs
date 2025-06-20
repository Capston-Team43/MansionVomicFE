using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    [Header("�þ� ����")]
    public float viewAngle = 120f;             // �þ߰�
    public float viewDistance = 10f;           // �þ� �Ÿ�

    [Header("���̾� ����")]
    public LayerMask targetMask;               // �÷��̾� ���̾�
    public LayerMask obstacleMask;             // ��/��ֹ� ���̾�

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

        // �þ� ���� �� ���� Ȯ��
        if (distToPlayer > viewDistance || angleToPlayer > viewAngle / 2f)
            return false;

        // ��ֹ� ����ĳ��Ʈ �˻�
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, viewDistance, targetMask | obstacleMask))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // �����Ϳ��� �þ� Ȯ�ο� Gizmos
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
