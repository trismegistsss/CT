using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace CT.Objects.Weapons
{
   [Serializable]
   public struct GunPoints : ISerializable
    {
        [SerializeField] private GameObject _startbulletPoint;
        [SerializeField] private GameObject _finishBulletPoint;

        public GameObject StartbulletPoint
        {
            get { return _startbulletPoint; }
        }
        public GameObject FinishBulletPoint
        {
            get { return _finishBulletPoint; }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("startBulletPoint", _startbulletPoint);
            info.AddValue("finishBulletPoint", _finishBulletPoint);
        }
    }

    public class Weapon : MonoBehaviour
    {    
        [Header("ui icon:")]
        [SerializeField] private Sprite _icon;

        [Header("Gun Points:")]
        [SerializeField] private List<GunPoints> _gunPointsPool;

        [Header("Prameters:")]
        [SerializeField] private int _damage;
        [SerializeField] private int _maxBulletsInShoot;
        [SerializeField] private bool _inMomentShoot;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletDistantion = 3f;

        [Header("Bullet parameters:")]
        [SerializeField] private float _scaleBullet = 1f;

        public string NameWeapon { set; get; }

        void Awake()
        {
            if (null == _gunPointsPool) return;

            foreach (var gunPoints in _gunPointsPool)
            {
                var ob1 = gunPoints.StartbulletPoint;
                var ob2 = gunPoints.FinishBulletPoint;

                if(null==ob1 || null == ob2)
                    continue;

                if (ob1.transform.localPosition.x != ob2.transform.localPosition.x)
                {
                    var xlp1 = ob1.transform.localPosition;
                    var xlp2 = ob2.transform.localPosition;

                    if(xlp1.x < xlp2.x)
                        ob2.transform.localPosition = new Vector3(xlp1.x + _bulletDistantion, xlp2.y, xlp2.z);
                    else
                        ob2.transform.localPosition = new Vector3(xlp1.x - _bulletDistantion, xlp2.y, xlp2.z);
                }
                else if (ob1.transform.localPosition.y != ob2.transform.localPosition.y)
                {
                    var ylp1 = ob1.transform.localPosition;
                    var ylp2 = ob2.transform.localPosition;

                    if (ylp1.y < ylp2.y)
                        ob2.transform.localPosition = new Vector3(ylp1.x , ylp2.y + _bulletDistantion, ylp2.z);
                    else
                        ob2.transform.localPosition = new Vector3(ylp1.x, ylp2.y - _bulletDistantion, ylp2.z);
                }
            }
        }

        public Sprite Icon
        {
            get {return _icon; }
        }

        public List<GunPoints> GamePointsPool
        {
            get { return _gunPointsPool; }
        }

        public int Damage
        {
            get { return _damage; }
        }

        public int MaxBulletInShot
        {
            get { return _maxBulletsInShoot; }
        }

        public bool InMomentShot
        {
            get { return _inMomentShoot; }
        }

        public float Speed
        {
            get { return _bulletSpeed; }
        }

        public float Distantion
        {
            get { return _bulletDistantion; }
        }

        public float Scale
        {
            get { return _scaleBullet; }
        }
    }
}
