using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;
    
    [System.Serializable]
    public class Ability
    {
        public string abilityName;
        public bool isUnlocked;
        public int unlockStoryPoint;
    }
    
    public List<Ability> abilities = new List<Ability>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void UnlockAbility(string abilityName)
    {
        Ability ability = abilities.Find(a => a.abilityName == abilityName);
        if (ability != null)
        {
            ability.isUnlocked = true;
            Debug.Log($"Ability unlocked: {abilityName}");
        }
    }
    
    public bool IsAbilityUnlocked(string abilityName)
    {
        Ability ability = abilities.Find(a => a.abilityName == abilityName);
        return ability != null && ability.isUnlocked;
    }
    
    // Call this when player reaches a story milestone
    public void ReachStoryPoint(int storyPoint)
    {
        foreach (Ability ability in abilities)
        {
            if (ability.unlockStoryPoint == storyPoint && !ability.isUnlocked)
            {
                UnlockAbility(ability.abilityName);
            }
        }
    }
}