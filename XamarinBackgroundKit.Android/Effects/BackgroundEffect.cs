using System;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinBackgroundKit.Android.Effects;
using XamarinBackgroundKit.Android.Extensions;
using XamarinBackgroundKit.Effects;
using AView = Android.Views.View;

[assembly: ExportEffect(typeof(BackgroundEffectDroid), nameof(BackgroundEffect))]
namespace XamarinBackgroundKit.Android.Effects
{
	public class BackgroundEffectDroid : BasePlatformEffect<BackgroundEffect, Element, AView>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			ApplyGradient();
			ApplyRadius();
		}

		protected override void OnDetached()
		{
			base.OnDetached();

			if (View == null || View.Handle == IntPtr.Zero) return;

			View.Background?.Dispose();
		}

		#region Property Changed

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName == Background.GradientsProperty.PropertyName
				|| args.PropertyName == Background.AngleProperty.PropertyName
				|| args.PropertyName == Background.GradientTypeProperty.PropertyName) ApplyGradient();
			else if (args.PropertyName == Background.BorderColorProperty.PropertyName
				|| args.PropertyName == Background.BorderWidthProperty.PropertyName) ApplyBorder();
			else if (args.PropertyName == Background.CornerRadiusProperty.PropertyName) ApplyRadius();
		}

		#endregion

		#region Apply Radius and Gradient

		private void ApplyGradient()
		{
			if (View == null) return;

			var type = Background.GetGradientType(Element);
			var angle = Background.GetAngle(Element);
			var gradients = Background.GetGradients(Element);

			View.SetGradient(type, gradients, angle);
		}

		private void ApplyRadius()
		{
			if (!(Element is VisualElement viewElement)) return;

			View?.SetCornerRadius(View.Context, viewElement, Background.GetCornerRadius(Element));
		}

		private void ApplyBorder()
		{
			if (!(Element is VisualElement viewElement)) return;

			var borderColor = Background.GetBorderColor(Element);
			var borderWidth = Background.GetBorderWidth(Element);

			View?.SetBorder(View.Context, viewElement, borderColor, borderWidth);
		}

		#endregion
	}
}