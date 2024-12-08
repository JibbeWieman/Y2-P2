using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Manager
{
    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    // Reference to the player and player controller
    //public SceneTypeObject player;
    //private PlayerCharacterController playerController;

    public override void Start()
    {
        base.Start();

        //EventManager.AddListener<SkillUnlockedEvent>(IncreaseStat);
    }

    /* public override void Pause()
    {
        throw new System.NotImplementedException();
    } */

    /*
    /// <summary>
    /// Method to increase a specific stat type
    /// </summary>
    /// <param name="statType"></param>
    public void IncreaseStat(SkillUnlockedEvent evt)
    {
        SkillType statType = evt.skillType;
        switch (statType)
        {
            case SkillType.Health:
                //playerController.IncreaseHealth(10);
                Events.HPIncreasedEvent.HPBuff += 25;
                break;
            case SkillType.Speed:
                //playerController.IncreaseMovementSpeed(2f);
                Events.SpeedIncreasedEvent.SpeedBuff += 1;
                break;
            case SkillType.Strength:
                //playerController.IncreaseBlasterDMG(100);
                Events.BlasterDMGIncreasedEvent.DMGBuff += 10;
                break;
        }
    }
    */

    private void OnDestroy()
    {
        //EventManager.RemoveListener<SkillUnlockedEvent>(IncreaseStat);
    }
}
