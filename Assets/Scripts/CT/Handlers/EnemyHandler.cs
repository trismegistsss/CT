using System;
using CT.Config;
using CT.Factories;
using CT.Models;
using CT.Objects;
using CT.Objects.Enemy;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CT.Handlers
{
    public class EnemyHandler : IInitializable, IDisposable
    {
        [Inject] private GameManager _gameManager;
        [Inject] private GameModel _gameModel;
        [Inject] private EnemiesFactory _enemiesFactory;
        [Inject] private GameItemsInfo _gameItemsInfo;

        CompositeDisposable _disposables = new CompositeDisposable();

        private int _itemId;

        public void Dispose()
        {
            if (_disposables != null)
                _disposables.Dispose();
        }

        public void DestroyAllEnemies()
        {
            _gameItemsInfo.DestroyAllEnemyTanks();
        }

        public void DestroyEnemy(EnemyTank eTank)
        {  
            _gameItemsInfo.DestroyEnemyTank(eTank);
        }

        public void Initialize()
        {
            _itemId = 1;
        }

        public void InitEnemies()
        {
           var nideTanks =  GameConfig.GetCountEnemyInGame(_gameModel.Level.Value);
            if (_gameItemsInfo.EnemyTanks.Count >= nideTanks ) return;

            for (var i = nideTanks - _gameItemsInfo.EnemyTanks.Count; i>0;i--)
            {
                AddEnemy();
            }
        }

        private void AddEnemy()
        {
            var len = GameConfig.GetAvelablyEnemyInGame(_gameModel.Level.Value);
            var nEnemy = _enemiesFactory.InjectEnemy(len[Random.Range(0, len.Count)]);
            nEnemy.Id = _itemId;
            nEnemy.transform.SetParent(_gameManager.GamePresenter.transform);

            var r = Random.Range(0f, 2f);
            var p = Random.Range(0f, 2f);
            Vector3 randPos;
            if(r==0)
                randPos = Camera.main.ScreenToWorldPoint
                    (new Vector2((p == 0f? Screen.width:0f),Random.Range(0, Screen.height)));
            else
                randPos = Camera.main.ScreenToWorldPoint
                    (new Vector2((Random.Range(0, Screen.width)), (p == 0f ? Screen.height : 0f)));

            nEnemy.transform.position = new Vector3(randPos.x, randPos.y, -1);
            nEnemy.transform.localScale = Vector3.one;
            nEnemy.transform.Rotate(0, 0, Random.Range(0, 360));
            _gameItemsInfo.AddEnemyTank(nEnemy);

            _itemId++;
        }
    }
}