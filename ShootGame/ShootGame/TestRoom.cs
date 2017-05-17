//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;

//namespace ShootGame
//{
//    class TestRoom
//    {
//        public Hero Hero;
//        public GameForm Form;
//        public Level CurrentLevel;

//        [Test]
//        public void MoveHero()
//        {
//            InitField();
//            CurrentLevel.MoveHero(Form.Size, new bool[4] { true, true, false, false });
//            Assert.AreEqual(new Vector(5, 5), Hero.Location);
//        }

//        [Test]
//        public void RotateHero()
//        {
//            InitField();
//            CurrentLevel.RotateHero(new System.Drawing.Point(10, 10));
//            Assert.AreEqual(new Vector(5, 5), Hero.Location);
//        }

//        [Test]
//        public void EnemyAndHeroInteraction()
//        {
//            InitField();
//            var enemy = new Enemy(EName.robot0, new Vector(10, 10));
//            enemy.Move(new Hero(new Vector(5, 5)), 0);
//            Assert.AreNotSame(10, Hero.Health);
//        }

//        [Test]
//        public void HeroOutOfField()
//        {
//            var hero = new Hero(new Vector(0, 0));
//            var form = new GameForm(Levels.CreateLevels());
//            var currentLevel = new Level("level0", hero);
//            currentLevel.MoveHero(form.Size, new bool[4] { true, false, false, false });
//            Assert.AreEqual(new Vector(0, 0), hero.Location);
//        }

//        [Test]
//        public void TwoEnemyInteraction_Location()
//        {
//            InitField();
//            var firstEnemy = new Enemy(EName.robot0, new Vector(3 ,8));
//            var secondEnemy = new Enemy(EName.robot0, new Vector(5, 8));
//            firstEnemy.Move(new Hero(new Vector(3, 8)), 0);
//            secondEnemy.Move(new Hero(new Vector(5, 8)), 0);
//            Assert.AreNotEqual(firstEnemy.Location,secondEnemy.Location);
//        }

//        [Test]
//        public void TwoEnemyInteraction_Health()
//        {
//			InitField();
//            var firstEnemy = new Enemy(EName.robot0, new Vector(3, 8));
//            var secondEnemy = new Enemy(EName.robot1, new Vector(5, 8));
//			firstEnemy.Move(new Hero(new Vector(3, 8)), 0);
//			secondEnemy.Move(new Hero(new Vector(5, 8)), 0);
//            Assert.AreEqual(firstEnemy.Health,10);
//        }

//        [Test]
//        public void BulletOutOfField()
//        {
//			InitField();
//            var bullet = new Bullet(Weapon.UZI, new Vector(0, 0), -1);
//			bullet.Move();
//            Assert.AreEqual(false, Bullet.Bullets.Contains(bullet));
//        }

//        [Test]
//        public void TwoBulletsInteraction()
//        {
//            InitField();
//            var firstBullet = new Bullet(Weapon.UZI, new Vector(0, 1), 0);
//            var secondBullet = new Bullet(Weapon.UZI, new Vector(0, 10), 0);
//            firstBullet.Move();
//            secondBullet.Move();
//            Assert.AreEqual(false,Bullet.Bullets.Contains(firstBullet) && Bullet.Bullets.Contains(secondBullet));
//        }


//        public void InitField()
//        {
//            Hero = new Hero(new Vector(5, 5), 10, 0, 0);
//            Form = new GameForm(Levels.CreateLevels());
//            CurrentLevel = new Level("level0", Hero);
//        }
//    }
//}
