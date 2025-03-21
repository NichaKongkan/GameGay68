using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    void Start()
    {
        CheckRelationshipAndWarp();
    }

    void CheckRelationshipAndWarp()
    {
        int boss1Relationship = PlayerPrefs.GetInt("RelationshipWithBoss_Boss_1", 0);
        int boss2Relationship = PlayerPrefs.GetInt("RelationshipWithBoss_Boss_2", 0);
        int boss3Relationship = PlayerPrefs.GetInt("RelationshipWithBoss_Boss_3", 0);


        Debug.Log("Relation Boss1: " + boss1Relationship);
        Debug.Log("Relation Boss2: " + boss2Relationship);
        Debug.Log("Relation Boss3: " + boss3Relationship);

        //>= 15
        if ((boss1Relationship+boss2Relationship+boss3Relationship) >= 15)
        {
            SceneManager.LoadScene("Ending1");
        }
        else
        {
            SceneManager.LoadScene("Ending2");
        }
    }
}
