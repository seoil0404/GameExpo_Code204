using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/CombatData")]
public class CombatData : ScriptableObject
{
    public EnemyData.HabitatType HabitatType;
    public EnemyData.EnemyType EnemyType;

    [SerializeField] private List<TreasureType> treasureData;

    public void Initialize()
    {
        HabitatType = EnemyData.HabitatType.Forest;
        EnemyType = EnemyData.EnemyType.Common;

        Debug.Log("Clear");
        treasureData = new List<TreasureType>();
    }

    private void Awake()
    {
        if(treasureData == null) treasureData = new List<TreasureType>();
    }

    public void AddTreasureData(TreasureType type)
    {
        foreach(TreasureType item in treasureData)
        {
            if (item == type) return;
        }

        treasureData.Add(type);

        Scene.treasureShowManager.UpdateTreasureImages();
    }

    public TreasureType[] TreasureData
    {
        get
        {
            return treasureData.ToArray();
        }
    }

    /// <summary>
    /// UniversalGravitation : �����η�
    /// BusinessAcumen : �������
    /// Condemnation : ����
    /// MoneyBack : �� �ָӴ�
    /// EmergencyFood : ��� �ķ�
    /// GiantResistanceHammer : ����ο� ö��
    /// </summary>
    [Serializable]
    public enum TreasureType
    { UniversalGravitation, BusinessAcumen, Condemnation, MoneyBack, EmergencyFood, GiantResistanceHammer }
}