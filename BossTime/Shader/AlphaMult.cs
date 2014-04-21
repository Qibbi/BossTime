using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace BossTime.Shader
{
	public class AlphaMultEffect : ShaderEffect
	{
		private static PixelShader _pixelShader;

		static AlphaMultEffect()
		{
			_pixelShader = new PixelShader();
			_pixelShader.UriSource = new Uri("pack://application:,,,/BossTime;component/Shader/AlphaMult.ps");
		}

		private static readonly DependencyProperty _inputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(AlphaMultEffect), 1);
		public Brush Input
		{
			get { return (Brush)GetValue(_inputProperty); }
			set { SetValue(_inputProperty, value); }
		}

		private static readonly DependencyProperty _alphaProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Alpha", typeof(AlphaMultEffect), 0);
		public Brush Alpha
		{
			get { return (Brush)GetValue(_alphaProperty); }
			set { SetValue(_alphaProperty, value); }
		}

		public AlphaMultEffect()
		{
			PixelShader = _pixelShader;
			Alpha = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/BossTime;component/Art/Textures/BossAlpha.png")));
			UpdateShaderValue(_inputProperty);
			UpdateShaderValue(_alphaProperty);
		}
	}
}
