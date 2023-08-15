#if !NET461
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Uno;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.UI;
using Windows.UI.Xaml.Media;

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
				if (_children == null || _children.Empty())
				{
					return;
				}
				if (Orientation == Orientation.Vertical)
				{
					foreach (var child in _children)
					{
						if (position.Y >= child.ActualOffset.Y + child.ActualSize.Y / 2)
						{
							first++;
						}
					}
				}
				else
				{
					foreach (var child in _children)
					{
						if (position.X >= child.ActualOffset.X + child.ActualSize.X / 2)
						{
							first++;
						}
					}
				}
				if (first + 1 < _children.Count)
				{
					second = first + 1;
				}
			}
		}
	}
}

#endif
