using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    public Transform firePoint; // ������Ʈ �߻� ��ġ
    public void FireSkill(string effectName,float finalDmg)
    {
        GameObject skillObj = ObjectPoolManager.instance.GetObject(effectName); // �Ű������� �̸� �޾ƿͼ� Ǯ��
        skillObj.transform.position = firePoint.position + new Vector3(0, 0.5f, 0);
        skillObj.transform.rotation = firePoint.rotation;
        SkillObject skill = skillObj.GetComponent<SkillObject>();
        skill.Init(finalDmg); 
    }
}
