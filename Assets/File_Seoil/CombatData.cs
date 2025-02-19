using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Scriptable Objects/CombatData")]
public class CombatData : ScriptableObject
{
    public EnemyData.HabitatType HabitatType;
    public EnemyData.EnemyType EnemyType;

    private List<TreasureType> treasureData;

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
    /// </summary>
    [Serializable]
    public enum TreasureType
    { UniversalGravitation, BusinessAcumen, Condemnation }
}
