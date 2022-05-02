#version 430 core
out vec4 outputColor;

in vec2 texCoord;

// Can use layout(binding = #) for static binding
//layout(binding = 0) uniform sampler2D texture1;
//layout(binding = 1) uniform sampler2D texture2;
uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
    outputColor = mix(texture(texture1, texCoord), texture(texture2, texCoord), 0.2);
//    outputColor =  texture(texture0, texCoord) * vec4(ourColor, 1.0);
}