using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace App3D;

public class Shader : IDisposable
{
	public ProgramHandle Handle { get; }
	private bool _disposedValue = false;
	// private readonly Dictionary<string, int> _uniformLocations;

	public void SetInt(string name, int value)
	{
		GL.UseProgram(Handle);
		
		// Sets up the texture sampler uniform
		// They are represented as simple int value
		int location = GL.GetUniformLocation(Handle, name);
		
		GL.Uniform1i(location, value);
	}

	public Shader(string vertexPath, string fragmentPath)
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		// Load the source code from the individual shader files
		//////////////////////////////////////////////////////////////////////////////////////////////////////

		var vertexShaderSource = File.ReadAllText(vertexPath);

		var fragmentShaderSource = File.ReadAllText(fragmentPath);

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
		// These two shaders must then be merged into a shader program, which can then be used by OpenGL.
		// To do this, create a program...
		Handle = GL.CreateProgram();

		// Attach both shaders...
		GL.AttachShader(Handle, vertexShader);
		GL.AttachShader(Handle, fragmentShader);

		// And then link them together.
		GL.LinkProgram(Handle);
		// When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
		// Detach them, and then delete them.
		GL.DetachShader(Handle, vertexShader);
		GL.DetachShader(Handle, fragmentShader);
		GL.DeleteShader(fragmentShader);
		GL.DeleteShader(vertexShader);
		// // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
		// // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
		// // later.
		// // First, we have to get the number of active uniforms in the shader.
		//
		// var numberOfUniforms = 0;
		// GL.GetProgrami(Handle, ProgramPropertyARB.ActiveUniforms, ref numberOfUniforms);
		//
		// // Next, allocate the dictionary to hold the locations.
		// _uniformLocations = new Dictionary<string, int>();
		//
		// // Loop over all the uniforms,
		// for (var i = 0; i < numberOfUniforms; i++)
		// {
		// 	// get the name of this uniform,
		//
		// 	var _ = 0;
		// 	var key = GL.GetActiveUniformName(Handle, Convert.ToUInt32(i), 1024, ref _);
		//
		// 	// get the location,
		// 	var location = GL.GetUniformLocation(Handle, key);
		//
		// 	// and then add it to the dictionary.
		// 	_uniformLocations.Add(key, location);
		// }
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