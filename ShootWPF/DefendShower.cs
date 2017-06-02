using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Threading.Tasks;


namespace ShootGame
{
    public abstract class DefendShower : Shower
    {
        protected int fireCount = 0;

        public DefendShower(Defend defend, string imgPath, DispatcherTimer timer)
            : base(defend, new StackPanel(), timer)
        {
            Image img = new Image() { Source = new BitmapImage(new Uri(imgPath, UriKind.Relative)) };
            img.Width = defend.Width;
            img.Height = defend.Height;
            StackPanel spanel = (StackPanel)Show;
            spanel.HorizontalAlignment = HorizontalAlignment.Left;
            spanel.Children.Add(img);
        }

        public abstract Shape GetBulletShape();

        public virtual BulletShower GetBullet()
        {
            return new BulletShower(((Defend)Role).Fire(), GetBulletShape(), Timer);
        }


        protected override void timer_Tick(object sender, EventArgs e)
        {
            if (Canv == null)
                return;
            base.timer_Tick(sender, e);
            Defend defend = (Defend)Role;
            if (defend.FireFrequency > 0 && (fireCount++) % defend.FireFrequency == 0)
            {
                defend.Fire();
                BulletShower bullet = this.GetBullet();
                Canvas.SetLeft(bullet.Show, defend.X + defend.FireX);
                Canvas.SetTop(bullet.Show, defend.Y + defend.FireY);
                Canv.Children.Add(bullet.Show);
            }
        }
        protected override void role_OnDestroyed(object sender, EventArgs e)
        {
            RotateTransform rotate = new RotateTransform();
            Show.RenderTransform = rotate;
            rotate.CenterX = Role.Width / 2;
            rotate.CenterY = Role.Height;
            DoubleAnimation ani = new DoubleAnimation(0, -90, new Duration(TimeSpan.FromSeconds(0.5)));
            ani.Completed += ani_Completed;
            rotate.BeginAnimation(RotateTransform.AngleProperty, ani);
        }

        void ani_Completed(object sender, EventArgs e)
        {
            Timer.Tick -= timer_Tick;
            if (Canv != null)
                Canv.Children.Remove(Show);
        }
    
    }
    public class HagShower : DefendShower
    {
        public HagShower(Hag hag, string imgPath, DispatcherTimer timer)
            : base(hag, imgPath, timer)
        { }
        public override Shape GetBulletShape()
        {
            Defend defender = (Defend)Role;
            return new Ellipse() { Fill = Brushes.Black };
        }
        
    }
    public class WarriorShower : DefendShower
    {
        public WarriorShower(Warrior warrior, string imgPath, DispatcherTimer timer)
            : base(warrior, imgPath, timer)
        { }
        public override Shape GetBulletShape()
        {
            Defend defender = (Defend)Role;
            return new Ellipse() { Fill = Brushes.Yellow };
        }
    }
}
