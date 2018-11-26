using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;
            var tableView = Control as UITableView;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            if (Control != null && Element != null)
            {
                UpdateUi((ExtendedListView)Element);
            }

        }

        private void UpdateUi(ExtendedListView element)
        {
            Control.ScrollEnabled = element.IsScrollEnable;
        }
    }
}

