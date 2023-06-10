#if !NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.UI;
using Windows.UI.Xaml.Media;
using static Uno.UI.FeatureConfiguration;

namespace Windows.UI.Xaml.Controls
{
	public partial class ItemsStackPanel : Panel, IVirtualizingPanel, IInsertionPanel
	{
		VirtualizingPanelLayout _layout;

#if UNO_REFERENCE_API
		[NotImplemented]
#endif
		public int FirstVisibleIndex => _layout?.FirstVisibleIndex ?? -1;
#if UNO_REFERENCE_API
		[NotImplemented]
#endif
		public int LastVisibleIndex => _layout?.LastVisibleIndex ?? -1;

#if XAMARIN_ANDROID
		public int FirstCacheIndex => _layout.XamlParent.NativePanel.ViewCache.FirstCacheIndex;
		public int LastCacheIndex => _layout.XamlParent.NativePanel.ViewCache.LastCacheIndex;
#endif

		public ItemsStackPanel()
		{
			if (FeatureConfiguration.ListViewBase.DefaultCacheLength.HasValue)
			{
				CacheLength = FeatureConfiguration.ListViewBase.DefaultCacheLength.Value;
			}

#if UNO_REFERENCE_API || __MACOS__
			CreateLayoutIfNeeded();
			_layout.Initialize(this);
#endif
		}

		VirtualizingPanelLayout IVirtualizingPanel.GetLayouter()
		{
			CreateLayoutIfNeeded();
			return _layout;
		}

		private void CreateLayoutIfNeeded()
		{
			if (_layout == null)
			{
				_layout = new ItemsStackPanelLayout();
				_layout.BindToEquivalentProperty(this, nameof(Orientation));
				_layout.BindToEquivalentProperty(this, nameof(AreStickyGroupHeadersEnabled));
				_layout.BindToEquivalentProperty(this, nameof(GroupHeaderPlacement));
				_layout.BindToEquivalentProperty(this, nameof(GroupPadding));
#if !XAMARIN_IOS
				_layout.BindToEquivalentProperty(this, nameof(CacheLength));
#endif
			}
		}

		void IInsertionPanel.GetInsertionIndexes(Windows.Foundation.Point position, out int first, out int second)
		{
			first = -1;
			second = -1;
			if ((new Windows.Foundation.Rect(0, 0, ActualSize.X, ActualSize.Y)).Contains(position))
			{
				List<UIElement> children = GetChildren().ToList();
				if (children == null || children.Empty())
				{
					return;
				}
				if (position.Y < children[0].ActualOffset.Y + children[0].ActualSize.Y / 2)
				{
					first = -1;
					second = 0;
					return;
				}
				for (int i = 1; i < children.Count; i++)
				{
					if ((position.Y >= children[i - 1].ActualOffset.Y + children[i - 1].ActualSize.Y / 2) && ((position.Y < children[i].ActualOffset.Y + children[i].ActualSize.Y / 2)))
					{
						first = i - 1;
						second = i;
						return;
					}
				}
				if (position.Y >= children[children.Count - 1].ActualOffset.Y + children[children.Count - 1].ActualSize.Y / 2)
				{
					first = children.Count - 1;
					second = children.Count;
					return;
				}
			}
		}
	}
} 

#endif
