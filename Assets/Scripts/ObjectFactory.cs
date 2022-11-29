using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class IItem
    {
        public GameObject ItemGO { get; set; }
    }
    public abstract class IGameFactory
    {
        public enum Item { Bullet, Enemy }
        public abstract IItem CreateItem(Item item, GameObject gameObject);
    }

    class Bullet : IItem
    {
        public float Speed { get; set; }
    }

    class EnemyObject : IItem
    {
        public float Health { get; set; }
    }

    public class ObjectFactory : IGameFactory
    {
        public override IItem CreateItem(Item item, GameObject gameObject)
        {
            IItem newItem = new IItem();
            switch (item)
            {
                case Item.Bullet:
                    Bullet bullet = new Bullet
                    {
                        ItemGO = gameObject,
                        Speed = Random.Range(4.0f, 5.0f)
                    };
                    return bullet;
                case Item.Enemy:
                    EnemyObject enemyObject = new EnemyObject
                    {
                        ItemGO = gameObject,
                        Health = 5f
                    };
                    return enemyObject;
                default:
                    break;
            }
            return newItem;
        }
    }
}

