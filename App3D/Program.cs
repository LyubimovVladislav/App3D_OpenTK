using System;

namespace App3D;

public class Program
{
	public static void Main(string[] args)
	{
		using Game game = new(800,600, "Test", 60d);
		game.Run();
	}
}