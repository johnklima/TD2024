using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite fullHeart;

    public Sprite nullHeart;
    private bool _hasInit;

    // leif edit: we already have a health system, where playerHealth etc is calculated
    // this is only for displaying x hearts for n health;
    // attached to Prefab "HealthDisplay"
    private Image[] _hearts;

    private int _maxNumberOfHearts;
    private int _playerHealth;
    private PlayerHealthSystem _playerHealthSystem; // leif edit

    #region LEIF TESTS

    [Serializable]
    public class TestSettings
    {
        public int aasd = 1;
    }


    public void TestAddHeart()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test during RunTime");
            return;
        }

        Debug.Log("TestAddHeart!");
        _hearts = _hearts.Concat(new[] { GenerateHeart() }).ToArray();
        _playerHealthSystem.maxHp = _hearts.Length;
    }

    public void TestRemoveHeart()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Can only test during RunTime");
            return;
        }

        Debug.Log("TestRemoveHeart!");

        var temp = new Image[_hearts.Length - 1];
        for (var i = 0; i < temp.Length; i++) temp[i] = _hearts[i];
        Destroy(_hearts[^1].gameObject);
        _hearts = temp;
        _playerHealthSystem.maxHp = _hearts.Length;
    }

    #endregion

    #region leif edit start

    private Image GenerateHeart(GameObject _gameObject = null)
    {
        var newHeart = _gameObject;
        if (newHeart == null) newHeart = new GameObject();
        newHeart.transform.parent = transform;
        var img = newHeart.AddComponent<Image>();
        img.sprite = fullHeart;
        return img;
    }

    public void UpdateHealthDisplay(int currentHp = -193)
    {
        // attached to PlayerHealthSystem in Start()
        if (currentHp != -193) _playerHealth = currentHp;
        for (var i = 0; i < _hearts.Length; i++) // iterate image[]
            if (i < _playerHealth)
                _hearts[i].sprite = fullHeart;
            else
                _hearts[i].sprite = nullHeart;
    }

    private void Start()
    {
        // get system
        _playerHealthSystem = FindObjectOfType<PlayerHealthSystem>();
        if (_playerHealthSystem == null)
            throw new Exception("Make sure there is a <PlayerHealthSystem> in the scene");

        // get maxHp and set new array
        _maxNumberOfHearts = _playerHealthSystem.maxHp;
        _hearts = new Image[_maxNumberOfHearts];
        // generate hearts
        for (var i = 0; i < _maxNumberOfHearts; i++) _hearts[i] = GenerateHeart();
        // get playerHealth to set correct start values
        _playerHealth = _playerHealthSystem.GetCurrentHp();
        // add listener to event on system
        _playerHealthSystem.AddUpdateHealthDisplayListener(UpdateHealthDisplay);
    }

    private void Update()
    {
        if (_hasInit) return;
        _hasInit = true;
        UpdateHealthDisplay(_playerHealth);
    }

    #endregion // leif edit end
}