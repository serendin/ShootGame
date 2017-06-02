using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShootGame
{
    public class Role
    {
        protected int life;
        public int Life {  get { return life; }
            set { 
                life = value;
                if (life <= 0)
                    Destroy();
            }
        }  
        public int Power { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public event EventHandler OnDestroyed;
        public EventHandler OnMoving;
        public virtual void Move()
        {
            X -= VX;
            if (OnMoving != null)
                OnMoving(this, null);
        }

        public Rect Region
        {
            get { return new Rect(X, Y, Width, Height); }
        }

        public void Destroy()
        {
            if (OnDestroyed != null)
                OnDestroyed(this, null);
        }

        public virtual bool touch(Role other)
        {
            return this.Region.IntersectsWith(other.Region);
        }

    }
    public class Attack : Role
    {
        public double ReVX { get; set; }
    }
    public abstract class Defend : Role
    {
        public double FireX { get; set; }
        public double FireY { get; set; }
        public int FireFrequency { get; set; }

        public abstract Bullet Fire();
        public EventHandler OnFiring { get; set; }

    }
    public class FiringBulletEventArgs : EventArgs
    {
        public Bullet Bullet { get; set; }
    }
    public class Bullet : Role
    {
        public Bullet() { }
        public override void Move()
        {
            X += VX;
            if (OnMoving != null)
                OnMoving(this, null);
        }
    }
    public class Pumpkin : Role
    {
        public double VY { get; set; }
        public Pumpkin()
        {
            VX = 0;
            VY = 3;
            Width = 70;
            Height = 90;
            Life = 5;
        }
        public override void Move()
        {
            Y-=VY;
            if (OnMoving != null)
                OnMoving(this, null);
        }
        public bool isClicked(Point p)
        {
            if (p.X < X + Width && p.X > X && p.Y < Y + Height && p.Y > Y)
            {
                return true;
            }
            return false;
        }
    }

    public class Hag : Defend
    {
        public Hag()
        {
            Width = 60;
            Height = 90;
            FireX = 45;
            FireY = 10;
            FireFrequency = 20;
            Life =200;
            VX = 0;
        }
        public override Bullet Fire()
        {
            Bullet bullet = new Bullet() { X = this.X + this.FireX, Y = this.Y + this.FireY, Width = 15, Height = 15, VX = 50, Power = 30 };
            if (OnFiring != null)
                OnFiring(this, new FiringBulletEventArgs() { Bullet = bullet });
            return bullet;
        }
    }
    public class Warrior : Defend
    {
        public Warrior()
        {
            Width = 60;
            Height = 90;
            FireX = 45;
            FireY = 20;
            FireFrequency = 30;
            Life = 150;
            VX = 0;
        }
        public override Bullet Fire()
        {
            Bullet bullet = new Bullet() { X = this.X + this.FireX, Y = this.Y + this.FireY, Width = 10, Height = 10, VX = 50, Power = 20 };
            if (OnFiring != null)
                OnFiring(this, new FiringBulletEventArgs() { Bullet = bullet });
            return bullet;
        }
    }
    public class Ghost : Attack
    {
        public Ghost() 
        {
            Width = 50;
            Height = 70;
            VX = 1;
            ReVX = 1;
            Life = 100;
            Power = 5;
        }
    }
    public class Devil : Attack
    {
        public Devil()
        {
            Width = 75;
            Height = 90;
            VX = 3;
            ReVX = 3;
            Life = 200;
            Power = 10;
        }
    }
}
