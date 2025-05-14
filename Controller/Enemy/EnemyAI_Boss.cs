using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum BossAnimationState { Idle, Walk, Attack1, Attack2, Attack3,Die }
public class EnemyAI_Boss : MonoBehaviour
{
    
    public Face faces;
    public GameObject SlimeBody;
    public GameObject SlimeHitbox;
    public GameObject Slime;
    public SkillCaster sc;
    public BossAnimationState currentState;
    public Enemy Boss;
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;
    private Material faceMaterial;
    public Boss_Slime_SlamAtk BSS;

    public bool isDamaged;
    public float traceTime = 0;
    public float hitboxHoldingTime = 0;
    public Vector3 SlamPoint;
    public bool isSlaming = false;
    public float slamRadious = 7;

    public float slamCoolTime = 0;
    private float slamCurTime = 0;
    public float rangedAtkCoolTime = 5;
    private float rangedAtkCurrentTime = 0;

    Vector3 SpawnPos;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SpawnPos = transform.position;
        Boss = GetComponent<Enemy>();
        faceMaterial = SlimeBody.GetComponent<SkinnedMeshRenderer>().materials[1];
        agent = gameObject.GetComponent<NavMeshAgent>();
        SlimeHitbox.SetActive(false);
    }
    
    void MonsterState()
    {
        
        switch (currentState)
        {
            case BossAnimationState.Idle:
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                AgentStop();
                animator.SetFloat("Speed",0);
                break;
            case BossAnimationState.Walk:
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;
                AgentStart();
                if (target != null) { TargetTrace(); } 
                else { return; }
                animator.SetFloat("Speed", agent.velocity.magnitude); 
                break;
            case BossAnimationState.Attack1:
               // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                SetFace(faces.AttackFace);
                animator.SetTrigger("Attack"); 
                hitboxHoldingTime = 0.1f;
                break;
            case BossAnimationState.Attack2:
                SetFace(faces.AttackFace);
                animator.SetTrigger("RangedAtk");
                break;
            case BossAnimationState.Attack3:    
                BSS.TrySlam();
                AgentStop();
                break;
            case BossAnimationState.Die:
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;
                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", 2);
                AgentStop();
                break;
           
        }
    }
    public void RangedAtk() // ���Ÿ� ���� AnimationEvent
    {
        Enemy enemy = GetComponent<Enemy>();
        sc.FireSkill("Boss_SlimeRA",enemy.atk);
        rangedAtkCurrentTime = 0;
    }
    public void BossStatusController()
    {
        float distance = Vector3.Distance(gameObject.transform.position, target.position);
        if (distance <= 2f && isDamaged == true) // ���ݹ޾�����, �Ÿ���� 
        {
            currentState = BossAnimationState.Attack1; //��������
        }
        else if (distance > 2f && isDamaged == true )                        // ���ݹ޾Ұ�, �������� �� �� �ִ� �Ÿ����� �ִٸ�,
        {
            if (rangedAtkCurrentTime > rangedAtkCoolTime && distance >= 8)   // ���Ÿ����� ��Ÿ�� �� �Ÿ��� �� �������ٸ� �÷��̾�� ���Ÿ����� 
                currentState = BossAnimationState.Attack2;

            else                                                             //�� ���� ��� �������� ������ �÷��̾� �Ѿư���
                currentState = BossAnimationState.Walk; 
        }
        else // �����ð����� ���ݹ��� �ʾƼ� isDamaged�� false�� �ȴٸ�
        {
            agent.SetDestination(SpawnPos);             // ������ �ʱ� ������ġ�� ���ϰ�
            currentState = BossAnimationState.Walk;     // �ʱ� ������ġ�� �ǵ��ư���
            if (transform.position == SpawnPos)         // �ʱ� ��ġ�� ���ư���
            {
                currentState = BossAnimationState.Idle; //���� idle
            }
        }
    }
    public void BssProcess()
    {
        if ((Boss.curHp <= Boss.maxHp / 2)&& slamCurTime>=slamCoolTime)
        {
            currentState = BossAnimationState.Attack3;
            slamCurTime = 0;
        }
    }
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
        
    }
    void CallHitBox()
    {
        if (hitboxHoldingTime >= 0)
        {
            SlimeHitbox.SetActive(true);
        }
        else if(hitboxHoldingTime<0)
        {
            SlimeHitbox.SetActive(false); 
        }
    }
    void TargetTrace()
    {
        if (isDamaged == true)
        {
            agent.SetDestination(target.position);
        }
    }
    public void TraceTimeZero ()
    {
        traceTime = 0;
    }
    void StopTrace()
    {
        if (traceTime <= 0)
        {
            isDamaged = false;
        }
    }
    
    void AgentStop()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
    }
    void AgentStart()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        hitboxHoldingTime -= Time.deltaTime;
        traceTime -= Time.deltaTime;
        rangedAtkCurrentTime += Time.deltaTime;
        slamCurTime += Time.deltaTime;
        MonsterState();
        StopTrace();
        BossStatusController();
        BssProcess();
        CallHitBox();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackBox"))
        {
            isDamaged = true;
            traceTime = 10f;
        }
    }
}
