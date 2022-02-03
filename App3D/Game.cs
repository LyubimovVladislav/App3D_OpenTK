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
		0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
		0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
		-0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
		-0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
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
	private Texture _texture = null!;
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
		if (KeyboardState[Keys.Escape])
		{
			Close();
		}

		base.OnUpdateFrame(args);
	}

	protected override void OnLoad()
	{
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		_shader = new Shader("shader.vert", "shader.frag");
		_vertexBufferObject = GL.GenBuffer();
		_vertexArrayObject = GL.GenVertexArray();
		_elementBufferObject = GL.GenBuffer();

		GL.BindVertexArray(_vertexArrayObject);

		GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);
		GL.BufferData(BufferTargetARB.ArrayBuffer, _vertices, BufferUsageARB.StaticDraw);

		GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
		GL.BufferData(BufferTargetARB.ElementArrayBuffer, _indices, BufferUsageARB.StaticDraw);

		// GL.VertexAttribPointer(_shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false,
		// 	3 * sizeof(float), 0);
		// GL.EnableVertexAttribArray(_shader.GetAttribLocation("aPosition"));

		var vertexLocation = _shader.GetAttribLocation("aPosition");
		GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
		GL.EnableVertexAttribArray(vertexLocation);
		
		var texLocation = _shader.GetAttribLocation("aTexCoord");
		GL.EnableVertexAttribArray(texLocation);
		GL.VertexAttribPointer(texLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

		_texture = new Texture(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"../../../Textures/container.jpg"));
		// int textureLocation = GL.GetUniformLocation(_shader.Handle, "texture0");
		// GL.Uniform4f(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
		
		// _timer.Start();

		base.OnLoad();
	}

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		// render
		// clear the color buffer
		GL.Clear(ClearBufferMask.ColorBufferBit);

		// be sure to activate the shader
		_shader.Use();

		// update the uniform color
		
		// double timeValue = _timer.Elapsed.TotalSeconds;
		// float greenValue = (float)Math.Sin(timeValue) / (2.0f + 0.5f);
		// int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
		// GL.Uniform4f(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);


		// now render the triangle
		GL.BindVertexArray(_vertexArrayObject);
		// GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
		GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);


		// swap buffers
		Context.SwapBuffers();
		base.OnRenderFrame(args);
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