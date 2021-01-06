using UnityEngine;
using System.Collections;

public class AdjustScript : MonoBehaviour {

	//void OnGUI()
 //   {
 //       //if (GUI.Button(new Rect(10, 100, 100, 30), "Health up"))
 //       //{
 //       //    // GameControl é static, por isso pode ser acessado diretamente
 //       //    // .control é o membro estático e a health é a variável pública
 //       //    GameControl.control.health += 10;
 //       //}
 //       //if (GUI.Button(new Rect(10, 140, 100, 30), "Health down"))
 //       //{
 //       //    GameControl.control.health -= 10;
 //       //}
        
 //       GUI.Label(new Rect(100, 550, 30, 30), "XP");
 //       if (GUI.Button(new Rect(100, 555, 30, 30), "+"))
 //       {
 //           GameControl.control.experience += 10;
 //       }
 //       if (GUI.Button(new Rect(130, 555, 30, 30), "-"))
 //       {
 //           GameControl.control.experience -= 10;
 //       }
 //       if (GUI.Button(new Rect(10, 260, 100, 30), "Save"))
 //       {
 //           GameControl.control.Save();
 //       }
 //       if (GUI.Button(new Rect(10, 300, 100, 30), "Load"))
 //       {
 //           GameControl.control.Load();
 //       }
 //   }

    public void XPUp()
    {
        GameControl.control.experience += 10;
    }

    public void XPDown()
    {
        GameControl.control.experience -= 10;
    }

    public void StrengthUp()
    {
        GameControl.control.strength += 1;
    }

    public void StrengthDown()
    {
        GameControl.control.strength -= 1;
    }

    public void DexterityUp()
    {
        GameControl.control.dexterity += 1;
    }

    public void DexterityDown()
    {
        GameControl.control.dexterity -= 1;
    }

    public void InteligenceUp()
    {
        GameControl.control.inteligence += 1;
    }

    public void InteligenceDown()
    {
        GameControl.control.inteligence -= 1;
    }

    public void ShieldBashUp()
    {
        GameControl.control.shieldBashSkill += 1;
    }

    public void ShieldBashDown()
    {
        GameControl.control.shieldBashSkill -= 1;
    }

    public void ImpaleUp()
    {
        GameControl.control.impaleSkill += 1;
    }

    public void ImpaleDown()
    {
        GameControl.control.impaleSkill -= 1;
    }

    public void MightyLeapUp()
    {
        GameControl.control.mightyLeapSkill += 1;
    }

    public void MightyLeapDown()
    {
        GameControl.control.mightyLeapSkill -= 1;
    }

    public void DeepSlashUp()
    {
        GameControl.control.deepSlashSkill += 1;
    }

    public void DeepSlashDown()
    {
        GameControl.control.deepSlashSkill -= 1;
    }

    public void SkillPointsUp()
    {
        GameControl.control.skillPointsRemaining += 1;
    }

    public void SkillPointsDown()
    {
        GameControl.control.skillPointsRemaining -= 1;
    }

    public void AttributePointsUp()
    {
        GameControl.control.attributePointsRemaining += 1;
    }

    public void AttributePointsDown()
    {
        GameControl.control.attributePointsRemaining -= 1;
    }
    
}
