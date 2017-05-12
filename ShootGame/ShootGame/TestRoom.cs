using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ShootGame
{
    class TestRoom
    {
        public Hero Hero;
        public GameForm Form;
        public Level CurrentLevel;

        [Test]
        public void MoveHero()
        {
            InitField();
            CurrentLevel.MoveHero(Form.Size, new bool[4] { true, true, false, false });
            Assert.AreEqual(new Vector(5, 5), Hero.Location);
        }

        [Test]
        public void RotateHero()
        {
            InitField();
            CurrentLevel.RotateHero(new System.Drawing.Point(10, 10));
            Assert.AreEqual(new Vector(5, 5), Hero.Location);
        }

        [Test]
        public void EnemyAndHeroInteraction()
        {
            InitField();
            var enemy = new Enemy(new Vector(10, 10));
            enemy.Move(new Vector(5, 5));
            Assert.AreNotSame(10, Hero.Health);
        }

        [Test]
        public void HeroOutOfField()
        {
            var hero = new Hero(new Vector(0, 0));
            var form = new GameForm(Levels.CreateLevels());
            var currentLevel = new Level("level0", hero);
            currentLevel.MoveHero(form.Size, new bool[4] { true, false, false, false });
            Assert.AreEqual(new Vector(0, 0), hero.Location);
        }

        [Test]
        public void TwoEnemyInteraction_Location()
        {
            InitField();
            var firstEnemy = new Enemy(new Vector(3,8));
            var secondEnemy = new Enemy(new Vector(5, 8));
            firstEnemy.Move(new Vector(3,8));
            secondEnemy.Move(new Vector(5,8));
            Assert.AreNotEqual(firstEnemy.Location,secondEnemy.Location);
        }

        [Test]
        public void TwoEnemyInteraction_Health()
        {
			InitField();
            var firstEnemy = new Enemy(Name.robot0, new Vector(3, 8), 10, 0, 0);
            var secondEnemy = new Enemy(Name.robot1, new Vector(5, 8), 10, 0, 0);
			firstEnemy.Move(new Vector(3, 8));
			secondEnemy.Move(new Vector(5, 8));
            Assert.AreEqual(firstEnemy.Health,10);
        }

        [Test]
        public void BulletOutOfField()
        {
			InitField();
            var bullet = new Bullet(new Vector(0, 0),0,-1, 100);
			bullet.Move();
            Assert.AreEqual(false, Bullet.Bullets.Contains(bullet));
        }

        [Test]
        public void TwoBulletsInteraction()
        {
            InitField();
            var firstBullet = new Bullet(new Vector(0, 1), 0, 0, 100);
            var secondBullet = new Bullet(new Vector(0, 10), 0, 0, 100);
            firstBullet.Move();
            secondBullet.Move();
            Assert.AreEqual(false,Bullet.Bullets.Contains(firstBullet) && Bullet.Bullets.Contains(secondBullet));
        }


        public void InitField()
        {
            Hero = new Hero(new Vector(5, 5),10,0,0);
            Form = new GameForm(Levels.CreateLevels());
            CurrentLevel = new Level("level0", Hero);
        }
    }
}
