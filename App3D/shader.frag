#version 330 core
out vec4 FragColor;

//uniform vec4 ourColor;
in vec3 ourColor;

void main()
{
    FragColor = vec4(ourColor, 1.0);
    //    FragColor = ourColor;
    //    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);

}