using System;
using Windows.Foundation;
using Windows.UI.Core;

namespace Uno.UI.Xaml.Controls;

internal partial class NativeWindowWrapper : NativeWindowWrapperBase
{
	private static readonly Lazy<NativeWindowWrapper> _instance = new(() => new NativeWindowWrapper());

	internal static NativeWindowWrapper Instance => _instance.Value;

	public override object NativeWindow => null;

	public override void Activate()
	{
	}

	public override void Close()
	{
	}

	internal void OnNativeClosed() => RaiseClosed();

	internal void OnNativeActivated(CoreWindowActivationState state) => ActivationState = state;

	internal void OnNativeVisibilityChanged(bool visible) => Visible = visible;

	internal void RaiseNativeSizeChanged(double width, double height)
	{
		var bounds = new Rect(default, new Size(width, height));

		Bounds = bounds;
		VisibleBounds = bounds;
	}

	protected override void ShowCore() => WindowManagerInterop.WindowActivate();
}
