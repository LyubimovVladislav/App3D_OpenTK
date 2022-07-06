using System.Diagnostics;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace App3D;

public class Game : GameWindow
{
	// private readonly float[] _vertices =
	// {
	// 	// positions        // colors
	// 	0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // top right
	// 	0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom right
	// 	-0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, // bottom left
	// 	-0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 0.0f // top left
	// };

	float[] _vertices =
	{
		//Position          Texture coordinates
		0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // top right
		0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
		-0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
		-0.5f, 0.5f, 0.0f, 0.0f, 1.0f // top left
	};

	private readonly uint[] _indices =
	{
		// note that we start from 0!
		0, 1, 3, // first triangle
		1, 2, 3 // second triangle
	};

	// private float[] _texCoords = {
	// 	0.0f, 0.0f,  // bottom-left corner  
	// 	1.0f, 0.0f,  // bottom-right corner
	// 	0.0f, 1.0f,  // top-left corner
	// 	1.0f, 1.0f   //top right
	// };


	private Shader _shader = null!;
	private Texture _texture1 = null!;
	private Texture _texture2 = null!;
	private BufferHandle _vertexBufferObject;
	private VertexArrayHandle _vertexArrayObject;
	private BufferHandle _elementBufferObject;
	private readonly Stopwatch _timer;

	public Game(int width, int height, string title, double renderFrequency) :
		base(new GameWindowSettings() { RenderFrequency = renderFrequency, UpdateFrequency = renderFrequency },
			new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title }
		)
	{
		int nrAttributes = 0;
		GL.GetInteger(GetPName.MaxVertexAttribs, ref nrAttributes);
		Console.WriteLine("Maximum number of vertex attributes supported: " + nrAttributes);
		_timer = new Stopwatch();
	}

	protected override void OnUpdateFrame(FrameEventArgs args)
	{
		base.OnUpdateFrame(args);
		if (KeyboardState[Keys.Escape])
		{
			Close();
		}
	}

	protected override void OnLoad()
	{
		base.OnLoad();

		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

		_vertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(_vertexArrayObject);

		_vertexBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);
		GL.BufferData(BufferTargetARB.ArrayBuffer, _vertices, BufferUsageARB.StaticDraw);

		_elementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
		GL.BufferData(BufferTargetARB.ElementArrayBuffer, _indices, BufferUsageARB.StaticDraw);

		_shader = new Shader("shader.vert", "shader.frag");
		_shader.Use();

		var vertexLocation = _shader.GetAttribLocation("aPosition");
		GL.EnableVertexAttribArray(vertexLocation);
		GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

		var texLocation = _shader.GetAttribLocation("aTexCoord");
		GL.EnableVertexAttribArray(texLocation);
		GL.VertexAttribPointer(texLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
			3 * sizeof(float));

		_texture1 = new Texture("Textures/container.jpg");
		// _texture1.Use(TextureUnit.Texture0);
		_texture2 = new Texture("Textures/awesomeface.png");
		// _texture2.Use(TextureUnit.Texture1);

		// This sets the uniform texture1 to use whatever is in texture unit 0, and texture2 to use whatever is in texture unit 1.

		//Before setting up the uniforms MAKE SURE TO "Use" the shader!!!
		_shader.SetInt("texture1", 0);
		_shader.SetInt("texture2", 1);

		// _timer.Start();
		
		// Make transformation matrix
		
		Matrix4d rotation = Matrix4d.RotateZ(MathHelper.RadiansToDegrees(90f));
		Matrix4d scale = Matrix4d.Scale(0.5f, 0.5f, 0.5f);
		Matrix4d trans = rotation * scale;
		
		_shader.Use();
		// Set transformation matrix as uniform
		int location = GL.GetUniformLocation(_shader.Handle, "transform");
		Console.WriteLine($"location: {location}");
		GL.UniformMatrix4d(location, true, in trans);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Console.Write($"{trans[i, j]} ");
			}
			Console.Write("\n");
		}
	}

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		base.OnRenderFrame(args);

		GL.Clear(ClearBufferMask.ColorBufferBit);
		GL.BindVertexArray(_vertexArrayObject);

		_texture1.Use(TextureUnit.Texture0);
		_texture2.Use(TextureUnit.Texture1);
		_shader.Use();

		GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

		Context.SwapBuffers();
	}

	protected override void OnResize(ResizeEventArgs e)
	{
		GL.Viewport(0, 0, e.Width, e.Height);
		base.OnResize(e);
	}

	protected override void OnUnload()
	{
		GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
		GL.DeleteBuffer(_vertexBufferObject);
		_shader.Dispose();

		// _timer.Stop();

		base.OnUnload();
	}
}