#version 430 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;// output a color to the fragment shader

uniform mat4 transform;

void main()
{
    texCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0) * transform;
}