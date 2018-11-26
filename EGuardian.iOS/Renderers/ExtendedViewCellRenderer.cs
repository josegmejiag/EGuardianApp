using System;
using System.Reflection;
using CoreGraphics;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            try
            {
                // This is the assembly full name which may vary by the Xamarin.Forms version installed.
                // NullReferenceException is raised if the full name is not correct.
                var globalContextViewCell = Type.GetType("Xamarin.Forms.Platform.iOS.ContextActionsCell, Xamarin.Forms.Platform.iOS, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null");

                // Now change the static field value! "NormalBackground" OR "DestructiveBackground"
                if (globalContextViewCell != null)
                {
                    var normalButton = globalContextViewCell.GetField("NormalBackground", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                    if (normalButton != null)
                    {
                        normalButton.SetValue(null, getImageBasedOnColor("48016B"));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in setting background color of Menu Item : " + e.ToString());
            }
            var view = item as ExtendedViewCell;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
            };

            return cell;
        }

        private UIImage getImageBasedOnColor(string colorCode)
        {
            // Get UIImage with a green color fill
            CGRect rect = new CGRect(0, 0, 1, 1);
            CGSize size = rect.Size;
            UIGraphics.BeginImageContext(size);
            CGContext currentContext = UIGraphics.GetCurrentContext();
            currentContext.SetFillColor(Color.FromHex(colorCode).ToCGColor());
            currentContext.FillRect(rect);
            var backgroundImage = UIGraphics.GetImageFromCurrentImageContext();
            currentContext.Dispose();

            return backgroundImage;
        }

    }
}
