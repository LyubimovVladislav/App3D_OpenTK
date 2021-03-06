#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
//in vec3 aPosition;
//in vec3 aColor;
//out vec4 vertexColor;

out vec3 ourColor; // output a color to the fragment shader

void main()
{
    gl_Position = vec4(aPosition, 1.0);
    ourColor = aColor;
    //    vertexColor = vec4(0.5, 0.0, 0.0, 1.0);
}