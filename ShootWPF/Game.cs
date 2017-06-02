using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShootGame
{
    public class Game
    {
        List<Pumpkin> pumpkins;
        public List<Pumpkin> Pumpkins
        {
            get { return pumpkins; }
        }
        List<Defend> defenders;
        public List<Defend> Defenders
        {
            get { return defenders; }
        }

        List<Attack> attackers;
        public List<Attack> Attackers
        {
            get { return attackers; }
        }

        List<Bullet> bullets;
        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public Game()
        {
            defenders  = new List<Defend>();
            attackers= new List<Attack>();
            bullets = new List<Bullet>();
            pumpkins = new List<Pumpkin>();
        }

        public bool AddPumpkin(Pumpkin pumpkin)
        {
            for (int i = pumpkins.Count - 1; i >= 0;i-- )
                if(pumpkin.touch(pumpkins[i]))
                    return false;
            pumpkin.OnDestroyed += pumpkin_OnDestroyed;
            pumpkins.Add(pumpkin);
            return true;
        }
        void pumpkin_OnDestroyed(object sender, EventArgs e)
        {
            pumpkins.Remove((Pumpkin)sender);
        }

        public void AddAttacker(Attack attack)
        {
            attack.OnMoving += attacker_OnMoving;   
            attack.OnDestroyed += attacker_OnDestroyed;
            attackers.Add(attack);
        }

        void attacker_OnMoving(object sender, EventArgs e)
        {
            Attack attack = sender as Attack;
            if (attack == null)
                return;
            for (int i = defenders.Count - 1; i >= 0; i--)
                if (attack.touch(defenders[i]))
                {
                    attack.VX = 0;
                    if (defenders[i].Life > attack.Power)
                        defenders[i].Life -= attack.Power;
                    else
                        defenders[i].Life = 0;
                    return;
                }
            if (attack.VX == 0)
            {
                attack.VX = attack.ReVX;
            }
        }

        void attacker_OnDestroyed(object sender, EventArgs e)
        {
            attackers.Remove((Attack)sender);
        }

        public bool AddDefender(Defend defend)
        {
            for (int i = defenders.Count - 1; i >= 0;i-- )
                if(defend.touch(defenders[i]))
                    return false;
            defend.OnFiring += defender_OnFiring;
            defend.OnDestroyed += defender_OnDestroyed;
            defenders.Add(defend);
            return true;
        }

        void defender_OnFiring(object sender, EventArgs e)
        {
            FiringBulletEventArgs args = e as FiringBulletEventArgs;
            if (e == null)
                return;
            bullets.Add(args.Bullet);
            args.Bullet.OnMoving += defendBullet_OnMoving;
        }

        void defendBullet_OnMoving(object sender, EventArgs e)
        {
            Bullet bullet = sender as Bullet;
            if (bullet == null)
                return;
            for (int i = attackers.Count - 1; i >= 0; i--)
                if (bullet.touch(attackers[i]))
                {
                    if (attackers[i].Life > bullet.Power)
                        attackers[i].Life -= bullet.Power;
                    else
                        attackers[i].Life = 0;
                    bullet.Destroy();
                }
        }

        void defender_OnDestroyed(object sender, EventArgs e)
        {
            defenders.Remove((Defend)sender);
        }

        public bool isClickPumpkin(Point p)
        {
            for(int i = pumpkins.Count - 1; i >= 0; i--)
                if (pumpkins[i].isClicked(p))
                {
                    pumpkins[i].Destroy();
                    return true;
                }
            return false;
        }

        public bool Isfalied()
        {
            for (int i = attackers.Count - 1; i >= 0; i--)
                if (attackers[i].X <= 0)
                    return true;
            return false;
        }
        public void Clear()
        {
            attackers.Clear();
            defenders.Clear();
            pumpkins.Clear();
        }

    }
}
