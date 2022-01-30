using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace App3D;

public class Shader : IDisposable
{
	public ProgramHandle Handle { get; }
	private bool _disposedValue = false;
	

	public Shader(string vertexPath, string fragmentPath)
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Load the source code from the individual shader files
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		string vertexShaderSource;
		using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
		{
			vertexShaderSource = reader.ReadToEnd();
		}

		string fragmentShaderSource;

		using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
		{
			fragmentShaderSource = reader.ReadToEnd();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Generate shaders handles and bind the source code to them
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		ShaderHandle vertexShader = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(vertexShader, vertexShaderSource);

		ShaderHandle fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragmentShaderSource);

		
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Compile and check for errors
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		GL.CompileShader(vertexShader);

		GL.GetShaderInfoLog(vertexShader, out string infoLogVert);
		if (infoLogVert != System.String.Empty)
			System.Console.WriteLine(infoLogVert);

		GL.CompileShader(fragmentShader);

		GL.GetShaderInfoLog(fragmentShader, out string infoLogFrag);

		if (infoLogFrag != System.String.Empty)
			System.Console.WriteLine(infoLogFrag);
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Link the shaders into a program that runs on gpu
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		
		Handle = GL.CreateProgram();

		GL.AttachShader(Handle, vertexShader);
		GL.AttachShader(Handle, fragmentShader);

		GL.LinkProgram(Handle);
		
		// Cleanup 
		GL.DetachShader(Handle, vertexShader);
		GL.DetachShader(Handle, fragmentShader);
		GL.DeleteShader(fragmentShader);
		GL.DeleteShader(vertexShader);
	}
	
	public void Use()
	{
		GL.UseProgram(Handle);
	}
	
	protected virtual void Dispose(bool disposing)
	{
		if (_disposedValue) return;
		GL.DeleteProgram(Handle);
		_disposedValue = true;
	}

	~Shader()
	{
		GL.DeleteProgram(Handle);
	}


	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	public uint GetAttribLocation(string attribName)
	{
		return Convert.ToUInt32(GL.GetAttribLocation(Handle, attribName));
	}
}