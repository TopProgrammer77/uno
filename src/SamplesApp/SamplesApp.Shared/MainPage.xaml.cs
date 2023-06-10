using SampleControl.Presentation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewItem = Microsoft.UI.Xaml.Controls.TreeViewItem;
using TreeViewNode = Microsoft.UI.Xaml.Controls.TreeViewNode;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;

namespace SamplesApp
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();

			//sampleControl.DataContext = new SampleChooserViewModel(sampleControl);
			StackPanel panel = new StackPanel();
			TreeView tv = new TreeView
			{
				RootNodes =  {
					new TreeViewNode { Content = "aaa"},
					new TreeViewNode
					{
						Content = "bbb",
						Children =
						{
							new TreeViewNode { Content = "bbb-1" },
							new TreeViewNode { Content = "bbb-2" },
							new TreeViewNode { Content = "bbb-3" },
						}
					},
					new TreeViewNode { Content = "ccc" },
					new TreeViewNode { Content = "ddd" },
				},
			};
			tv.ItemsSource = null;

			//panel.Children.Add(tv);


			TextBlock tb = new TextBlock();
			tb.CanDrag = true;
			//panel.Children.Add((TextBlock)tb);

			ListView lv = new ListView
			{
				CanDragItems = true,
				CanReorderItems = true,
				AllowDrop = true,
				ItemsSource = new string[]
				{
					"1111",
					"2222",
					"3333",
					"4444",
				}
			};
			bool isPressed = false;
			tv.PointerMoved += (s, e) =>
			{
				if (!isPressed)
					return;
				Point point = e.GetCurrentPoint(this).Position;
				tb.Text = point.ToString();
				IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(point, tv, true);
				foreach (var element in elements)
				{
					tb.Text += "\n";
					tb.Text += VisualTreeHelper.GetParent(element).GetHashCode() + "------- ";
					tb.Text += element.ToString();
					tb.Text += "--------" + VisualTreeHelper.GetChildrenCount(element);
					if (element is TreeViewItem item)
					{
						if (item.Content is TreeViewNode node)
							tb.Text += "--------" + node.Content.ToString();
					}

				}
				//double minY = point.Y;
				//for(int i = 1; i< 4; i++)
				//{
				//    double y = 0;
				//}
			};
			tv.AddHandler(PointerPressedEvent, new PointerEventHandler((s, e) =>
			{
				isPressed = true;
				Point point = e.GetCurrentPoint(this).Position;
				tb.Text = point.ToString();
				IEnumerable<UIElement> elements = VisualTreeHelper.FindElementsInHostCoordinates(point, tv, true);
				foreach (var element in elements)
				{
					tb.Text += "\n";
					tb.Text += VisualTreeHelper.GetParent(element).GetHashCode() + "------- ";
					tb.Text += element.ToString();
					tb.Text += "--------" + VisualTreeHelper.GetChildrenCount(element);
					tb.Text += "------layoutslot: " + element.LayoutSlot;
					tb.Text += "------Transformbounds: " + element.TransformToVisual(null).TransformBounds(element.LayoutSlot);
					tb.Text += "------Transformbounds: " + element.TransformToVisual(null).TransformBounds(new Rect(0, 0, element.ActualSize.X, element.ActualSize.Y));
					if (element is TreeViewItem item)
					{
						if (item.Content is TreeViewNode node)
							tb.Text += "--------" + node.Content.ToString();
					}

				}
				//double minY = point.Y;
				//for(int i = 1; i< 4; i++)
				//{
				//    double y = 0;
				//}
			}), true);

			tv.AddHandler(PointerReleasedEvent, new PointerEventHandler((s, e) =>
			{
				isPressed = false;
			}), true);
			lv.PointerMoved += (s, e) =>
			{
				tb.Text = e.GetCurrentPoint(lv).Position.ToString();
			};
			panel.Children.Add(lv);
			panel.Children.Add(tv);
			panel.Children.Add(tb);

			tb.Text += VisualTreeHelper.GetChildrenCount(panel);
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(tv); i++)
			{
				tb.Text += VisualTreeHelper.GetChild(tv, i);
			}

			Content = panel;
		}
	}
}
