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
    public abstract class AttackShower : Shower 
    {

        public AttackShower(Attack attack, string imgPath, DispatcherTimer timer)
            : base(attack, new StackPanel(), timer)
        {
            Image img = new Image() { Source = new BitmapImage(new Uri(imgPath, UriKind.Relative)) };
            img.Width = attack.Width;
            img.Height = attack.Height;
            StackPanel spanel = (StackPanel)Show;
            spanel.HorizontalAlignment = HorizontalAlignment.Left;
            spanel.Children.Add(img);
        }
        protected override void role_OnDestroyed(object sender, EventArgs e)
        {
            RotateTransform rotate = new RotateTransform();
            Show.RenderTransform = rotate;
            rotate.CenterX = Role.Width / 2;
            rotate.CenterY = Role.Height;
            DoubleAnimation ani = new DoubleAnimation(0, 90, new Duration(TimeSpan.FromSeconds(0.5)));
            ani.Completed += ani_Completed;
            rotate.BeginAnimation(RotateTransform.AngleProperty, ani);
        }

        void ani_Completed(object sender, EventArgs e)
        {
            Timer.Tick -= timer_Tick;
            if (Canv != null)
                Canv.Children.Remove(Show);
        }
        protected override void timer_Tick(object sender, EventArgs e)
        {
            if (Canv == null)
                return;
            Attack attack = (Attack)Role;
            attack.Move();
            Canvas.SetLeft(Show, Role.X);
            Canvas.SetTop(Show, Role.Y);
        }

    }
    public class GhostShower : AttackShower
    {
        public GhostShower(Ghost ghost, string imgPath, DispatcherTimer timer)
            : base(ghost, imgPath, timer)
        { }
    }
    public class DevilShower : AttackShower
    {
        public DevilShower(Devil devil, string imgPath, DispatcherTimer timer)
            : base(devil, imgPath, timer)
        { }
    }

}
