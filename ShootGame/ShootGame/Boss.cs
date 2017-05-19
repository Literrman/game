using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootGame
{
    public class Boss
    {
        public Vector Location;
        public int Health;
        public double Direction;
        public const int HitBox = 30;
        public readonly Image BossImage;
        public int Damage;
        public readonly int Experiens;
        private const int StepLen = 3;
        //public string BossImage;

        public Boss(Vector location, int health, double direction, int damage, int exp)
        {
            Location = location;
            Health = health;
            Direction = direction;
            Damage = damage;
            Experiens = exp;
            BossImage = GetImage("enemy_00.png");
            //BossImage = "enemy_00";
        }

        public void Move(Hero hero, int timeCount)
        {
            Direction = (hero.Location - Location).Angle;
            //EnemyIMG = GetImage(EnemiesIMG[Name][animation++ / 10]);
            //animation %= 50;

            if ((Location - hero.Location).Length < Hero.HitBox)
            {
                //EnemyIMG = GetImage(EnemiesIMG[Name][animation2++ / 10 + 5]);
                //animation2 %= 40;
                hero.Health -= timeCount % 500 == 0 ? Damage : 0;
            }
            else Location += 0.5 * new Vector(Math.Cos(Direction), Math.Sin(Direction));

            foreach (var bull in Bullet.Bullets.ToList())
            {
                if ((Location - bull.Location).Length >= HitBox) continue;
                Bullet.Bullets.Remove(bull);
                Health -= bull.Damage;
                if (Health <= 0)
                {
                    hero.Experiens += Experiens;
                    //Enemies.Remove(this);
                    //EnemyIMG = GetImage(EnemiesIMG[Name][9]);
                    //Blood.Enqueue(this);
                    //Count++;
                    //if (Blood.Count == 50) Blood.Dequeue();
                }
                break;
            }
        }
        public static Bitmap GetImage(string e) => (Bitmap)ResourceManager.GetObject(e);
    }
}
