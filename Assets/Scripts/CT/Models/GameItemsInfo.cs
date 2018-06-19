using System.Collections.Generic;
using CT.Objects;
using CT.Objects.Enemy;
using UnityEngine;
using Zenject;

namespace CT.Models
{
    public class GameItemsInfo : IInitializable
    {
        public List<EnemyTank> EnemyTanks { get; private set; }
        public UserTank UserTank { get; private set; } 

        public void Initialize()
        {
            EnemyTanks = new List<EnemyTank>();
        }

        public void AddEnemyTank(EnemyTank eTank)
        {
            EnemyTanks.Add(eTank);
        }

        public void DestroyEnemyTank(EnemyTank eTank)
        {
            if (EnemyTanks.Contains(eTank))
            {
                GameObject.Destroy(eTank.gameObject);
                EnemyTanks.Remove(eTank);
            }
        }

        public void DestroyAllEnemyTanks()
        {
            EnemyTanks.RemoveAll(pr =>
            {
                GameObject.Destroy(pr.gameObject);
                return true;
            });

            EnemyTanks.Clear();
        }

        public void AddUserTank(UserTank uTank)
        {
            UserTank = uTank;
        }

        public void DestroyUserTank()
        {
            if (null != UserTank)
            {
                GameObject.Destroy(UserTank.gameObject);
                UserTank = null;
            }
        }
    }
}