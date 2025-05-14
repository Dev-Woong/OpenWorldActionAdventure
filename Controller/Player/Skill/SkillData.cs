using UnityEngine;
public enum SkillType
{
    SingleTarget,
    MultiTarget,
    AOE
}
public enum SkillTarget
{
    Player,
    Enemy
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SKill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public SkillTarget skilltarget;
    public string SkillName; // ��ų�̸� => ���� ���� �ʿ������
    //public Sprite Icon;  // ��ų ������ => UI�� �÷����� ��ų�̹��� (���� ������)
    //public float coolDown; // ��Ÿ�� => Ȯ�� ����
    [Header("������Ʈ Ǯ���� ������ ��Ʈ����")]
    public string effectName; // ��ų ����Ʈ �̸� => ������Ʈ Ǯ���� �����ð�. ���ʿ���
    public float lifeTime; // ��ų ���ӽð�
    public float speed; // ��ų ���ư��� �ӵ�
    
    [Header("��ų ������ ������(damage*Atk)")]
    public float damage; // ��ų ������
    
}
