﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace App3D;

public class Texture
{
	private int Handle { get; }

	public Texture(String path)
	{
		Handle = GL.GenTexture();
		Use();
		SetUp(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../", path));

	}

	public void Use(TextureUnit unit = TextureUnit.Texture0)
	{
		GL.ActiveTexture(unit);
		GL.BindTexture(TextureTarget.Texture2D, Handle);
	}

	private void SetUp(String path)
	{
		// GL.BindTexture(TextureTarget.Texture2d, Handle);
		//Load the image
		Image<Rgba32> image = Image.Load<Rgba32>(path);

		//ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
		//This will correct that, making the texture display properly.
		image.Mutate(x => x.Flip(FlipMode.Vertical));

		//Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
		var pixels = new List<byte>(4 * image.Width * image.Height);

		for (int y = 0; y < image.Height; y++)
		{
			var row = image.GetPixelRowSpan(y);

			for (int x = 0; x < image.Width; x++)
			{
				pixels.Add(row[x].R);
				pixels.Add(row[x].G);
				pixels.Add(row[x].B);
				pixels.Add(row[x].A);
			}
		}

		//Set wrap mode
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

		//Set texture filtering mode
		// GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
		// GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

		//Generate texture
		unsafe
		{
			fixed (byte* p = pixels.ToArray())
			{
				IntPtr ptr = (IntPtr)p;
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
					PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
			}
		}

		//Generate mipmaps
		// GL.GenerateMipmap(TextureTarget.Texture2d);
		GL.GenerateTextureMipmap(Handle);
	}
}