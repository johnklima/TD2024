using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CurseOMeter : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite doneSprite;
    [ReadOnly] [SerializeField] private Potion _requiredPotion;
    public GameObject doneSpriteObject;
    private SpriteRenderer _spriteRenderer;

    private Dictionary<string, Sprite> _sprites;

    public DummyTarget DummyTarget { get; set; }

    private void Start()
    {
        Initialize();
    }

    public void OnValidated()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (DummyTarget == null) DummyTarget = GetComponentInParent<DummyTarget>();
        if (DummyTarget == null) DummyTarget = GetComponentInParent<DummyTarget>();
        if (DummyTarget != null)
            _requiredPotion = DummyTarget.requiredPotion;

        if (sprites.Length > 0)
            MakeSpriteDictionary();

        if (sprites.Length > 0 && _spriteRenderer != null)
            _spriteRenderer.sprite = GetSpriteForPotion(_requiredPotion);

        if (doneSpriteObject == null) throw new Exception("need sprite obj");
        doneSpriteObject.SetActive(false);
    }


    private void MakeSpriteDictionary()
    {
        Dictionary<string, Sprite> spriteDict = new();
        for (var i = 0; i < sprites.Length; i++)
            spriteDict[sprites[i].name] = sprites[i];
        _sprites = spriteDict;
    }

    private Sprite GetSpriteForPotion(Potion potion)
    {
        var spriteName = potion switch
        {
            Potion.RedSkull => "Red_Skull_Potion",
            Potion.RedSun => "Red_Sun_Potion",
            Potion.RedWhirlpool => "Red_Whirlpool_Potion",
            Potion.RedRed => "Red_Red_Potion",
            Potion.YellowSkull => "Yellow_Skull_Potion",
            Potion.YellowSun => "Yellow_Sun_Potion",
            Potion.YellowWhirlpool => "Yellow_Whirlpool_Potion",
            Potion.YellowYellow => "Yellow_Yellow_Potion",
            Potion.BlueSkull => "Blue_Skull_Potion",
            Potion.BlueSun => "Blue_Sun_Potion",
            Potion.BlueWhirlpool => "Blue_Whirlpool_Potion",
            Potion.BlueBlue => "Blue_Blue_Potion",
            _ => throw new Exception("No sprite found for :" + potion)
        };

        if (_sprites.TryGetValue(spriteName, out var sprite))
            return sprite;
        throw new Exception("No sprite found for :" + spriteName);
    }

    public void PlantDestroyed()
    {
        transform.parent = null;
        doneSpriteObject.SetActive(true);
    }
}