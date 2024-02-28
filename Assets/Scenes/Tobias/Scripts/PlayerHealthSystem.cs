using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    //! leif edit: attach this script to PLAYER

    public int maxHp = 5; // leif edit (made public, set in editor on PLAYER)
    public int testAmount = 1; //leif edit;
    public UnityEvent Die = new(), onHeal = new(), onDamage = new();

    private readonly UnityEvent<int>
        _updateHealthDisplay = new(); // added int to event, made private (dont need public)

    private int _currentHp; // leif edit  (made private)

    private void Awake()
    {
        _currentHp = maxHp; // leif edit
    }


    public void AddUpdateHealthDisplayListener(UnityAction<int> action)
    {
        _updateHealthDisplay.AddListener(action);
    }

    public void TestTakeDamage() //leif edit: made for testing (buttons in editor)
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test while RunTime;");
            return;
        }

        TakeDamage(testAmount);
    }

    public void TestHeal() //leif edit: made for testing (buttons in editor)
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test while RunTime;");
            return;
        }

        Heal(testAmount);
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        //Just some paranoia
        if (_currentHp < 0) _currentHp = 0;

        _updateHealthDisplay.Invoke(_currentHp);
        if (_currentHp <= 0) Die.Invoke();
    }

    public void Heal(int healAmount)
    {
        _currentHp += healAmount;
        if (_currentHp > maxHp) _currentHp = maxHp;
        _updateHealthDisplay.Invoke(_currentHp);
    }

    //Currently used to update the UI in the HealthTest Script, might be useful for the actual UI.
    public int GetCurrentHp()
    {
        return _currentHp;
    }

    //If the player's current hp is equal to the max hp, then the player shouldn't be able to heal themself.
    //This disallows wasting healing items.
    public bool CanHeal()
    {
        return _currentHp < maxHp;
    }
}