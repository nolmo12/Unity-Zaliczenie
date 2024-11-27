using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public TMP_Text[] HudTexts;
    void Update()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
            return;
        }

        HudTexts[0].text = player.getHealth().ToString();

        Item CurItem = player.CurItem.Value;

        string ItemString = CurItem.Name;

        if(CurItem is RangedWeapon weapon)
        {
            ItemString +=" " + weapon.GetStats();
        }

        ItemString += $"\n{CurItem.Description}";

        HudTexts[1].text = ItemString;
        HudTexts[2].text = GameDirector.Instance.getHowMuchTimeHasPassed().ToString();
        HudTexts[3].text = GameDirector.Instance.getHowManyEnemiesSlain().ToString();

        HudTexts[4].text = $"{GameDirector.Instance.getHowManyEnemiesSlain() * 100 + GameDirector.Instance.getHowMuchTimeHasPassed()}";
    }
}
